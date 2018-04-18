var app = angular.module("app", ["angular-growl", "login", "user"]);

app.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }); };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
})

.controller("mainController", ["$scope", "$location", "$http", "growl", "Excel", "$timeout", function ($scope, $location, $http, growl, Excel, $timeout) {
    var vm = this;

    vm.CancelMoto = {};
    vm.VoidMoto = {};

    //APPROVED REPORT
    $scope.GetApprovedReport = function () {
        $http({
            method: "POST",
            url: "/Report/GetApproved",
            data: {
                start: $("#approvedStart").val(),
                end: $("#approvedEnd").val()
            }
        }).then(function (data) {
            vm.ApprovedReport = data.data.approved;

            $("#approvedReportModal").modal('hide');

            setTimeout(function () {
                $scope.exportToExcel('#approvedReportTable');
            }, 2000);
        });
    };

    $scope.GetAllMotoReport = function () {
        $http({
            method: "POST",
            url: "/Report/GetAllMoto",
            data: {
                start: $("#allStart").val(),
                end: $("#allEnd").val()
            }
        }).then(function (data) {
            vm.AllMotoReport = data.data.moto;

            $("#allReportModal").modal('hide');

            setTimeout(function () {
                $scope.exportToExcel('#allMotoReportTable');
            }, 2000);
        });
    };

    //==============

    ErrorMessage = function (message) {
        growl.error(message, { title: "Error!", ttl: 4000 });
    };

    SuccessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    };

    $scope.exportToExcel = function (tableId) { // ex: '#my-table'
        var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        $timeout(function () { location.href = exportHref; }, 100); // trigger download
    };

    $scope.CurrencyDropDown = [
        { value: "PHP", label: "PHP" },
        { value: "USD", label: "USD" }
    ];

    $scope.Logout = function () {
        $http({
            method: "POST",
            url: "/Home/Logout",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                window.location.href = "/Home/Login";
            }
        });
    };

    $scope.Init = function () {
        $http({
            method: "POST",
            url: "/Home/GetCurrentUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            vm.CurrentUser = data.data;
        });
    };

    //==========MOTO REQUEST===========

    $scope.ClearMotoForm = function () {
        vm.Form = {};
        vm.Form.ClientCode = "";
        vm.Form.PaxName = "";
        vm.Form.PaxFirstName = "";
        vm.Form.PaxLastName = "";
        vm.Form.RecordLocator = "";
        vm.Form.Currency = "";
        vm.Form.BCDFee = "";
        vm.totalAmount = 0;
    };

    $scope.InitMotoRequest = function () {
        $scope.ClearMotoForm();

        $http({
            method: "POST",
            url: "/Home/GetDropDowns",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error !== "") {
                ErrorMessage(data.data);
            }
            else {
                vm.ClientDropDown = data.data.clientDropDown;
            }
        });
    };

    $scope.BCDFeeChange = function () {
        $("#VATCheckBox").prop("checked", false);

        vm.Form.IfVAT = false;

        $scope.ComputeAdminFee();
    };

    $scope.ComputeVAT = function () {
        if (vm.Form.IfVAT === true) {
            vm.Form.BCDFee = Number((vm.Form.BCDFee * 1.12).toFixed(2));
        }
        else {
            vm.Form.BCDFee = Number((vm.Form.BCDFee / 1.12).toFixed(2));
        }

        $scope.ComputeAdminFee();
    };

    $scope.ComputeAdminFee = function () {
        vm.totalAmount = 0;

        $http({
            method: "POST",
            url: "/Home/ComputeAdminFee",
            data: {
                _clientCode: vm.Form.ClientCode,
                _airFare: vm.Form.Amount,
                _serviceFee: vm.Form.BCDFee,
                _otherFee: vm.Form.Others
            }
        }).then(function (data) {
            if (data.data.adminFee !== 0) {
                vm.Form.AdminFee = data.data.adminFee;

                $("#ManualComp").hide();
                $("#AutoComp").show();
            }

            vm.totalAmount = data.data.total;

            if (data.data.error !== "") {
                ErrorMessage(data.data.error);

                $("#ManualComp").show();
                $("#AutoComp").hide();
            }
        });
    };

    $scope.ComputeTotal = function () {
        vm.totalAmount = 0;

        $http({
            method: "POST",
            url: "/Home/ComputeTotal",
            data: {
                amount: vm.Form.Amount,
                serviceFee: vm.Form.BCDFee,
                otherFee: vm.Form.Others,
                adminFee: vm.Form.AdminFee
            }
        }).then(function (data) {
            vm.totalAmount = data.data;
        });
    }

    $scope.SaveMotoRequest = function (value) {
        var error = "";

        if (value.ClientCode === "" || value.ClientCode === null) {
            ErrorMessage("Client code is required");

            error = "Y";
        }

        if (value.PaxName === "" || value.PaxName === null) {
            ErrorMessage("Passenger Name is required");

            error = "Y";
        }

        //if (value.RecordLocator === "" || value.RecordLocator === null) {
        //    ErrorMessage("Record Locator is required");
        //    error = "Y";
        //}

        if (value.Currency === "" || value.Currency === null) {
            ErrorMessage("Currency is required");

            error = "Y";
        }

        if (value.Amount === "" || value.Amount === null) {
            ErrorMessage("Amount is required");

            error = "Y";
        }

        if (value.BCDFee === "" || value.BCDFee === null) {
            ErrorMessage("BCD Fee is required");

            error = "Y";
        }

        if (value.AdminFee === "" || value.AdminFee === null) {
            ErrorMessage("Admin Fee is required");

            error = "Y";
        }

        if (error === "") {
            $http({
                method: "POST",
                url: "/Home/GetDuplicate",
                data: { moto: value }
            }).then(function (data) {
                if (data.data.error !== "") {
                    ErrorMessage(data.data.error);
                }
                else {
                    if (data.data.duplicate.length === 0) {
                        $scope.SaveMoto(value);
                    }
                    else {
                        vm.Duplicate = data.data.duplicate;

                        $("#dupbutton").click();
                    }
                }
            });

        }
    };

    $scope.SaveMoto = function (value) {
        value.Status = "P";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: value }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage("Moto Request Sent (Please expect action within 24-48 hours)");

                $scope.ClearMotoForm();

                $("#DuplicateModal").modal('hide');
            }
        });
    };
    //=========END OF REQUEST============

    //=========PENDING MOTO REQUEST===========
    $scope.AssignToDelete = function (value) {
        vm.CancelMoto = value;
    };

    $scope.CancelMotoRequest = function () {
        vm.CancelMoto.Status = "X";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: vm.CancelMoto }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                $("#CancelMoto").modal('hide');

                $scope.InitPendingMoto();

                SuccessMessage("Successfully Canceled Moto Request");
            }
        });
    };

    $scope.InitPendingMoto = function () {
        $http({
            method: "POST",
            url: "/Home/GetPending",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error !== "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Request = data.data.request;
            }
        });
    };
        
    setInterval($scope.InitPendingMoto, 2000);

    $scope.ReFile = function (value) {
        vm.Form = value;

        vm.Form.ID = {};

        vm.Form.Status = "P";

        $("#RequestMenu").click();
    };

    $scope.ViewMoto = function (value) {
        vm.ViewModal = value;
    };

    $scope.DeclinedVoid = function (value) {
        value.Status = "D";

        if (value.DeclinedVoidedReason === "") {
            ErrorMessage("Declined Reason is required");
        }
        else {
            $http({
                method: "POST",
                url: "/Home/SaveMoto",
                data: { moto: value }
            }).then(function (data) {
                SuccessMessage("Moto Request Declined Successfully");

                $("#viewModal").modal('hide');

                value = {};
            });
        }
    };

    $scope.ApproveVoid = function (value) {
        value.Status = "V";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: value }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage("Moto Request Voided - " + value.RecordLocator);

                $("#viewModal").modal('hide');

                value = {};
            }
        });
    };

    $scope.VoidMoto = function () {
        vm.VoidMoto.Status = "F";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: vm.VoidMoto }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage("Moto Void Request Sent");

                $("#VoidMoto").modal('hide');

                vm.VoidMoto = {};
            }
        });
    };

    $scope.AsignToVoide = function (value) {
        vm.VoidMoto = value;
    };

    $scope.ApproveMoto = function (value) {
        var error = "";

        if (value.ApprovalCode === "" || value.ApprovalCode === null) {
            ErrorMessage("Approval Code is required");

            error = "Y";
        }

        if (error === "") {
            value.Status = "A";

            $http({
                method: "POST",
                url: "/Home/SaveMoto",
                data: { moto: value }
            }).then(function (data) {
                if (data.data !== "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Moto Request Approved - " + value.RecordLocator);

                    $("#viewModal").modal('hide');

                    value = {};
                }
            });
        }
    };

    $scope.DeclineMoto = function (value) {
        var error = "";

        if (value.DeclinedReason === "" || value.DeclinedReason === null) {
            ErrorMessage("Declined Reason is required");

            error = "Y";
        }

        if (error === "") {
            value.Status = "D";

            $http({
                method: "POST",
                url: "/Home/SaveMoto",
                data: { moto: value }
            }).then(function (data) {
                if (data.data !== "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Moto Request Declined - " + value.RecordLocator);

                    $("#viewModal").modal('hide');

                    value = {};
                }
            });
        }
    };
    //=========END OF PENDING MOTO REQUEST==========

    //=========DECLINED MOTO===============
    $scope.InitDeclined = function () {
        $http({
            method: "POST",
            url: "/Home/GetDeclinedPerUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error) {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Declined = data.data.declined;
            }
        });
    };

    setInterval($scope.InitDeclined, 2000);
    //=========END OF DECLINED MOTO==========

    //=========APPROVED MOTO===============
    $scope.InitApproved = function () {
        $http({
            method: "POST",
            url: "/Home/GetApprovedPerUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error) {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Approved = data.data.approved;
            }
        });
    };

    setInterval($scope.InitApproved, 2000);
    //=========END OF APPROVED MOTO==========

    //=========VOID MOTO=========
    $scope.initVoid = function () {
        $http({
            method: "POST",
            url: "/Home/GetVoidedPerUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error) {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Voided = data.data.voided;
            }
        });
    };

    setInterval($scope.initVoid, 2000);
    //=========END VOID==========

    //========CHANGE PASSWORD========
    $scope.ChangePass = function (value) {
        if (value.NewPass !== value.ConfirmPass) {
            ErrorMessage("Password Not Match!");

            value.NewPass = value.ConfirmPass = "";
        }
        else {
            $http({
                method: "POST",
                url: "/Home/ChangePassword",
                data: { user: value }
            }).then(function (data) {
                if (data.data !== "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Password sucessfully changed");

                    $("#ChangePassModal").modal('hide');

                    vm.Pass = {};
                }
            });
        }
    };
}]);