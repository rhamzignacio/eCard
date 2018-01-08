angular.module("user", ["angular-growl"])

.controller("userController", ["$scope", "$location", "$http", "growl", function ($scope, $location, $http, growl) {
    var vm = this;

    ErrorMessage = function (message) {
        growl.error(message, { title: "Error!", ttl: 4000 });
    }

    SuccessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    }

    $scope.TypeDropDown = [
        { value: "USR", label: "Requestor" },
        { value: "APR", label: "Approver" },
        { value: "ADM", label: "Admin" }
    ]

    $scope.DepartmentDropDown = [
        { value: "ACCTG", label: "Accounting" },
        { value: "AM", label: "Account Management" },
        { value: "BLD", label: "Business & Leissure" },
        { value: "BSI", label: "Business Solutions & Innovation" },
        { value: "DOCU", label: "Documentation" },
        { value: "MICE", label: "Mice" },
        { value: "SMMP", label: "SMMP" },
        { value: "TRANS", label: "Transport" }
    ];

    $scope.init = function () {
        $http({
            method: "POST",
            url: "/User/GetAll",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Users = data.data.users;
            }
        });
    }

    $scope.AssignReset = function (value) {
        vm.Reset = value;
    }

    $scope.AssignEdit = function (value) {
        vm.Modal = value;
    }

    $scope.AssignLock = function (value) {
        vm.Lock = value;
    }

    $scope.LockUnlock = function () {
        var message = "";

        if (vm.Lock.Status == "Y") {
            vm.Lock.Status = "N";

            message = "User sucessfully locked";
        }
        else {
            vm.Lock.Status = "Y";
            
            message = "User sucessfully unlocked";
        }

        $http({
            method: "POST",
            url: "/User/Save",
            data: { user: vm.Lock }
        }).then(function (data) {
            if (data.data != "") {
                ErrorMessage(data.data);
            }
            else {
                SuccessMessage(message);
            }
        });
    }

    $scope.ResetPassword = function () {
        var error = "N";

        if (vm.Reset.Password === "" || vm.Reset.Password === null) {
            error = "Y";

            ErrorMessage("Password is required");
        }

        if (vm.Reset.ConfirmPassword === "" || vm.Reset.ConfirmPassword === null) {
            error = "Y";

            ErrorMessage("Confirm password is required");
        }

        if (error === "N") {
            $http({
                method: "POST",
                url: "/User/Save",
                data: { user: vm.Reset }
            }).then(function (data) {
                if (data.data !== "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Successfully Reset Password");

                    $("#ResetModal").modal('hide');
                }
            });
        }
    }

    $scope.SaveUser = function () {
        var error = "N";

        if (vm.Modal.Username === "" || vm.Modal.Username === null) {
            error = "Y";

            ErrorMessage("Username is required");
        }

        if (vm.Modal.FirstName === "" || vm.Modal.LastName === null) {
            error = "Y";

            ErrorMessage("First name is required");
        }

        if (vm.Modal.LastName === "" || vm.Modal.LastName === null) {
            error = "Y";

            ErrorMessage("Last Name is required");
        }

        if (vm.Modal.Type === "" || vm.Modal.Type === null) {
            error = "Y";

            ErrorMessage("Type is required");
        }

        if (error === "N") {
            $http({
                method: "POST",
                url: "/User/Save",
                data: { user: vm.Modal }
            }).then(function (data) {
                if (data.data !== "") {
                    ErrorMessage(data.data);
                }
                else {
                    SuccessMessage("Sucessfully Saved");

                    $("#UserModal").modal('hide');

                    $scope.init();
                }
            });
        }
    }

}]);