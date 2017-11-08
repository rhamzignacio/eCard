var app = angular.module("app", ["angular-growl", "login"])

.controller("mainController", ["$scope", "$location", "$http", "growl", function ($scope, $location, $http, growl) {
    var vm = this;

    var Duplicate = "N";

    vm.CancelMoto = {};
    vm.VoidMoto = {};

    ErrorMessage = function (message) {
        growl.error(message, { title: "Error!", ttl: 4000 });
    }

    SuccessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    }

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
            if (data.data != "") {
                ErrorMessage(data.data);
            }
            else {
                window.location.href = "/Home/Login";
            }
        });
    }

    $scope.Init = function () {
        $http({
            method: "POST",
            url: "/Home/GetCurrentUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            vm.CurrentUser = data.data;
        });
    }

    //==========MOTO REQUEST===========

    $scope.ClearMotoForm = function () {
        vm.Form = {};
        vm.Form.ClientCode = "";
        vm.Form.PaxName = "";
        vm.Form.PaxFirstName = "";
        vm.Form.PaxLastName = "";
        vm.Form.RecordLocator = "";
        vm.Form.Curreny = "";
        vm.Form.Amount = "";
        vm.Form.BCDFee = "";
        vm.Form.AdminFee = "";
    }

    $scope.InitMotoRequest = function () {
        $scope.ClearMotoForm();

        $http({
            method: "POST",
            url: "/Home/GetDropDowns",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data);
            }
            else {
                vm.ClientDropDown = data.data.clientDropDown;
            }
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

        if (value.RecordLocator === "" || value.RecordLocator === null) {
            ErrorMessage("Record Locator is required");

            error = "Y";
        }

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
            Duplicate = "Y";
            $http({
                method: "POST",
            })

            if (Duplicate === "N") {
                value.Status = "P";

                $http({
                    method: "POST",
                    url: "/Home/SaveMoto",
                    data: { moto: value }
                }).then(function (data) {
                    if (data.data != "") {
                        ErrorMessage(data.data);
                    }
                    else {
                        SuccessMessage("Moto Request Sent");

                        $scope.ClearMotoForm();
                    }
                });
            }
        }
    }
    //=========END OF REQUEST============

    //=========PENDING MOTO REQUEST===========
    $scope.AssignToDelete = function (value) {
        vm.CancelMoto = value;
    }

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
    }

    $scope.InitPendingMoto = function () {
        $http({
            method: "POST",
            url: "/Home/GetPending",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Request = data.data.request;
            }
        });
    }

    setInterval($scope.InitPendingMoto, 2000);

    $scope.ReFile = function (value) {
        vm.Form = value;

        vm.Form.ID = {};

        vm.Form.Status = "P";

        $("#RequestMenu").click();
    }

    $scope.ViewMoto = function (value) {
        vm.ViewModal = value;
    }

    $scope.ApproveVoid = function (value) {
        value.Status = "V";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: value }
        }).then(function (data) {
            if (data.data != "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage("Moto Request Voided - " + value.RecordLocator);

                $("#viewModal").modal('hide');

                value = {};
            }
        });
    }

    $scope.VoidMoto = function () {
        vm.VoidMoto.Status = "F";

        $http({
            method: "POST",
            url: "/Home/SaveMoto",
            data: { moto: vm.VoidMoto }
        }).then(function (data) {
            if (data.data != "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage("Moto Void Request Sent");

                $("#VoidMoto").modal('hide');

                vm.VoidMoto = {};
            }
        });
    }

    $scope.AsignToVoide = function (value) {
        vm.VoidMoto = value;
    }

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
                data: { moto: value}
            }).then(function (data) {
                if (data.data != "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Moto Request Approved - " + value.RecordLocator);

                    $("#viewModal").modal('hide');

                    value = {};
                }
            })
        }
    }

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
                if (data.data != "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Moto Request Declined - " + value.RecordLocator);

                    $("#viewModal").modal('hide');

                    value = {};
                }
            });
        }
    }
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
    }

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
    }

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
    }

    setInterval($scope.initVoid, 2000);
    //=========END VOID==========

    //========CHANGE PASSWORD========
    $scope.ChangePass = function (value) {
        if (value.NewPass != value.ConfirmPass) {
            ErrorMessage("Password Not Match!");

            value.NewPass = value.ConfirmPass = "";
        }
        else {
            $http({
                method: "POST",
                url: "/Home/ChangePassword",
                data: { user: value }
            }).then(function (data) {
                if (data.data != "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Password sucessfully changed");

                    $("#ChangePassModal").modal('hide');

                    vm.Pass = {};
                }
            });
        }
    }
}]);