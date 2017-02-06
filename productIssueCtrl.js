/// <reference path="../ang/angular.js" />

angular.module('productIssue', [])
    .service('commonService',
        function($http) {
            var fac = {};

            fac.GetString = function (key) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetResourceString',
                    data: { 'key': key }
                });
            }
            fac.GetSerialNumber = function () {
                return $http({
                    method: 'GET',
                    url: '/Store/GetIssueSerialNo'
                });
            }
            fac.GetSuppliers = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetSuppliers'
                });
            }
            fac.GetUnits = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetUnits'
                });
            }
            fac.GetGrades = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetGrades'
                });
            }
            fac.GetDryingTypes = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetDryingTypes'
                });
            }
            fac.GetSupplierCategories = function (supplierId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetSuppliersCategories',
                    data: { 'supplierId': supplierId }
                });
            }

            fac.GetStoreCategories = function (storeId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetStoreCategories',
                    data: {'storeId': storeId }
                });
            }

            fac.GetStores = function (categoryId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetStores',
                    data: { 'categoryId': categoryId }
                });
            }
            fac.GetAllStores = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetAllStores'
                });
            }
            fac.GetIssueTypes = function () {
                return $http({
                    method: 'GET',
                    url: '/Common/GetIssueTypes'
                });
            }

            fac.GetProductsByStoreAndCategory = function (storeId, categoryId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetProductsByStoreAndCategory',
                    data: {'storeId': storeId, 'categoryId': categoryId }
                });
            }

            fac.GetProductSizeInM3 = function (productId, storeId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetProductQty',
                    data: { 'productId': productId, 'storeId': storeId }
                });
            }
            fac.GetGradeQty = function (gradeId, productId, storeId) {
                return $http({
                    method: 'POST',
                    url: '/Common/GetGradeQty',
                    data: { 'gradeId': gradeId, 'productId': productId, 'storeId': storeId }
                });
            }
            fac.CheckAvailability = function (isTemper, isLogs, isFireWood, storeId, details) {
                return $http({
                    method: 'POST',
                    url: '/Store/CheckProductAvailability',
                    data: { 'isTemper': isTemper, 'isLogs': isLogs, 'isFireWood': isFireWood, 'storeId': storeId, 'details': details }
                });
            }
            return fac;
        })
     .service('productService', function ($http, $q) {
         var fac = {};
         fac.UploadDocs = function (files,id) {
             var formData = new FormData();
             for (var i in files) {
              formData.append("files", files[i]);
             }
             formData.append("productIssueId", id);

             var defer = $q.defer();
             $http.post("/Store/UploadTransactionDocs", formData, {
                 withCredentials: true,
                 headers: { "Content-Type": undefined },
                 transformRequest: angular.identity
             }).success(function (d) {
                 defer.resolve(d);

             }).error(function (d) {
                 defer.reject("rejected!");

             });

             return defer.promise;
         }

         fac.IssueProducts = function (issuedProduct, productsDetails) {
             return $http({
                 method: 'POST',
                 url: '/Store/IssueProductsFromStore',
                 data: { 'issuedProduct': issuedProduct, 'productsDetails': productsDetails }
             });
         }

         return fac;
     }).filter('round', function () {
         return function (input) {
             if (input == null || input == undefined || input == 0) {
                 return input;
             }
             var n = Number(input)
             return n.toFixed(2);
         };
     })
.controller('productIssueCtrl',
    ['$scope', '$http', 'commonService','productService',
        function ($scope, $http, commonService, productService) {
            alertify.set('notifier', 'position', 'top-left');

            $scope.IssueVoucherNo = null;
            $scope.SerialNoTypeId = "3";
            $scope.serialNo = null;
            $scope.Suppliers = null;
            $scope.Categories = null;
            $scope.SupplierId = null;
            $scope.Stores = null;
            $scope.StoreId = null;
            $scope.templateUrl = null;
            $scope.productName = null;
            $scope.nonWoodProducts = [];
            $scope.woodProducts = [];
            $scope.productsDetails = [];
            $scope.issuedProduct = {};
            $scope.ProductId = null;
            $scope.qty = null;
            $scope.UnitId = null;
            $scope.totalQty = 0;
            $scope.GradeId = null;
            $scope.Length = null;
            $scope.Width = null;
            $scope.Thickness = null;
            $scope.DryingId = null;
            $scope.Steaming = null;
            $scope.cat = null;
            $scope.hasImage = false;
            $scope.disableStore = false;
            $scope.selectedFiles = [];
            $scope.OrderedM3 = 0;
            $scope.OrderedNo= 0;
            $scope.pnote = null;
            $scope.totalInM3 = false;
            $scope.AdditionTypeId = null;

            commonService.GetSerialNumber()
                .then(function(d) {
                    $scope.IssueVoucherNo = d.data;
                });
            commonService.GetIssueTypes()
                .then(function(d) {
                    $scope.IssueTypes = d.data;
                });
           
            commonService.GetUnits()
              .then(function (d) {
                  $scope.Units = d.data;
              });
            commonService.GetGrades()
              .then(function (d) {
                  $scope.Grades = d.data;
              });
            commonService.GetDryingTypes()
             .then(function (d) {
                 $scope.DryingTypes = d.data;
             });
            commonService.GetAllStores()
                  .then(function (d) {
                      $scope.Stores = d.data;
                  });

            $scope.onProductChange = function () {
                $scope.GradeName = null;

                commonService.GetProductSizeInM3($scope.ProductId, $scope.StoreId)
                   .then(function (d) {
                       $scope.totalInM3 = d.data.m;
                       $scope.ProductTotalQty = d.data.Total;
                       $scope.ProductQtyInThisStore = d.data.StoreTotal;
                   });
            };

            $scope.onGradeChange = function () {
                $scope.GradeName = $("#GradeId :selected").text();

                // temporary
                $scope.GradeQty = 1;

                //commonService.GetGradeQty($scope.GradeId, $scope.ProductId, $scope.StoreId)
                //.then(function (d) {
                //    $scope.GradeQty = d.data;
                //});
            };

            $scope.onCategoryChange = function () {
                $scope.templateUrl = null;
                
                $("#turl").html('<i style="margin-left: 44%; margin-top: 4%;" class="fa fa-spinner fa-pulse fa-4x fa-fw"></i>');
                $scope.cat = $("#CategoryId :selected").text();

                $scope.Products = null;
                $scope.GradeName = null;
                $scope.GradeId = null;

                if ($scope.CategoryId == "1002") {
                    $scope.templateUrl = '/Store/NonWoodDetails';
                } else if ($scope.CategoryId == "1") {
                    $scope.templateUrl = '/Store/LogsDetails';
                } else if ($scope.CategoryId == "2") {
                    $scope.templateUrl = '/Store/TimberDetails';
                } else if ($scope.CategoryId == "3") {
                    $scope.templateUrl = '/Store/FirewoodDetails';
                }

                commonService.GetProductsByStoreAndCategory($scope.StoreId, $scope.CategoryId)
                    .then(function (d) {
                        $scope.Products = d.data;
                    });

            };

            $scope.onStoreChange = function () {
               
                $scope.CategoryId = null;
                $scope.GradeName = null;

                commonService.GetStoreCategories($scope.StoreId)
                .then(function (d) {
                    $scope.Categories = d.data;
                });

                clearList();
            };
          
            function pushWoodProductToList() {
                var productName = $("#ProductId :selected").text();
                var grade = $("#GradeId :selected").text();
                var drying = $("#DryingId :selected").text();
                $scope.disableStore = true;
                
                $scope.woodProducts.push({
                    SerialNoTypeId: $scope.SerialNoTypeId,
                    SerialNo: $scope.serialNo,
                    IssueVoucherNo: $scope.IssueVoucherNo,
                    Date: $scope.Date,
                    StoreId: $scope.StoreId,
                    ProdName: productName,
                    ProductId: $scope.ProductId,
                    Qty: $scope.qty,
                    GradeId: $scope.GradeId,
                    GradName: grade,
                    Diameter: $scope.Diameter,
                    Length: $scope.Length,
                    Width: $scope.Width,
                    Thickness: $scope.Thickness,
                    DryingId: $scope.DryingId,
                    DryingNam: drying,
                    Steaming: $scope.Steaming,
                    TotalSizeM3: $scope.TotalSize,
                    Note: $scope.pnote,
                    OrderedM3: $scope.OrderedM3,
                    OrderedNo: $scope.OrderedNo,
                    VariationM3: ($scope.OrderedM3 - $scope.TotalSize),
                    VariationNo: ($scope.OrderedNo - $scope.qty)

                });
                //$scope.totalWoodQty = $scope.sum($scope.woodProducts, "TotalSizeM3");

            };

            $scope.addTimberToProductList = function () {
                var exist = false;
                if ($scope.Steaming == null) {
                    $scope.Steaming = false;
                }
                
                if ($scope.ProductId !== null && $scope.qty !== null && $scope.GradeId !== null && $scope.Length !== null && $scope.Width !== null && $scope.Thickness !== null && $scope.DryingId !== null ) {
                    var prSize = (($scope.Width / 100) * ($scope.Length / 100) * ($scope.Thickness / 100) * $scope.qty);

                    var addedSize = 0;
                    if ($scope.woodProducts.length > 0) {
                        angular.forEach($scope.woodProducts, function (value, key) {
                            if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Width == $scope.Width && value.Thickness == $scope.Thickness && value.DryingId == $scope.DryingId && value.Steaming == $scope.Steaming) {
                                addedSize += value.TotalSizeM3;
                            }

                        });
                    }

                    $scope.checkDet = {
                        ProductId: $scope.ProductId,
                        GradeId: $scope.GradeId,
                        Length: $scope.Length,
                        Width: $scope.Width,
                        Thickness: $scope.Thickness,
                        DryingId: $scope.DryingId,
                        Steaming: $scope.Steaming,
                        Qty: $scope.qty

                    };
                    commonService.CheckAvailability(true, false,false, $scope.StoreId, $scope.checkDet).then(function (d) {

                        if (!d.data.status) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                            return;
                        }

                        if (d.data.total <= 0) {
                            alertify.error('there is not enough product in store');
                            return;
                        }
                        if (d.data.total > 0) {

                            var remain = d.data.total - addedSize;
                            if (remain < prSize) {
                                alertify.error('there is not enough product in store');
                                return;
                            }
                            else {
                                angular.forEach($scope.woodProducts, function (value, key) {
                                    if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Width == $scope.Width && value.Thickness == $scope.Thickness && value.DryingId == $scope.DryingId && value.Steaming == $scope.Steaming) {
                                        value.Qty += $scope.qty;
                                        value.OrderedM3 += $scope.OrderedM3;
                                        value.OrderedNo += $scope.OrderedNo;
                                        value.VariationM3 += $scope.VariationM3;
                                        value.VariationNo += $scope.VariationNo;
                                        value.TotalSizeM3 = (($scope.Width / 100) * ($scope.Length / 100) * ($scope.Thickness / 100) * $scope.qty);
                                        exist = true;
                                    }

                                });
                                $scope.TotalSize = (($scope.Width / 100) * ($scope.Length / 100) * ($scope.Thickness / 100) * $scope.qty);

                                if (exist == false) {
                                    pushWoodProductToList();
                                }

                                $scope.totalWoodQty = $scope.sum($scope.woodProducts, "TotalSizeM3");

                                clearInputs();
                            }
                        }
                    },
                       function (d) {
                           alertify.error('All fields marked with an asterisk (*) are required!!');
                       }
                   );

                  

                } else {
                    commonService.GetString("AllAsteriskRequired").then(function (d) {
                        alertify.error(d.data);
                    },
                        function (d) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                        }
                    );
                }
            }

            $scope.addFirewoodToProductList = function () {
                var exist = false;

                if ($scope.ProductId !== null && $scope.qty !== null && $scope.GradeId !== null && $scope.Length !== null && $scope.Diameter !== null && $scope.DryingId !== null) {
                    var prSize = (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);
                    var addedSize = 0;
                    if ($scope.woodProducts.length > 0) {
                        angular.forEach($scope.woodProducts, function (value, key) {
                            if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Diameter == $scope.Diameter && value.DryingId == $scope.DryingId) {
                                addedSize += value.TotalSizeM3;
                            }

                        });
                    }
                    $scope.checkDet = {
                        ProductId: $scope.ProductId,
                        GradeId: $scope.GradeId,
                        Length: $scope.Length,
                        Diameter: $scope.Diameter,
                        DryingId: $scope.DryingId,
                        Qty: $scope.qty

                    };
                    commonService.CheckAvailability(false, false, true, $scope.StoreId, $scope.checkDet).then(function (d) {

                        if (!d.data.status) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                            return;
                        }

                        if (d.data.total <= 0) {
                            alertify.error('there is not enough product in store');
                            return;
                        }
                        if (d.data.total > 0) {

                            var remain = d.data.total - addedSize;
                            if (remain < prSize) {
                                alertify.error('there is not enough product in store');
                                return;
                            }
                            else {
                                angular.forEach($scope.woodProducts, function (value, key) {
                                    if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Diameter == $scope.Diameter && value.DryingId == $scope.DryingId) {
                                        value.Qty += $scope.qty;
                                          value.OrderedM3 += $scope.OrderedM3;
                                        value.OrderedNo += $scope.OrderedNo;
                                        value.VariationM3 += $scope.VariationM3;
                                        value.VariationNo += $scope.VariationNo;
                                        value.TotalSizeM3 += (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);
                                        exist = true;
                                    }

                                });
                                $scope.TotalSize = (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);

                                if (exist == false) {
                                    pushWoodProductToList();
                                }

                                $scope.totalWoodQty = $scope.sum($scope.woodProducts, "TotalSizeM3");

                                clearInputs();
                            }
                        }
                    },
                       function (d) {
                           alertify.error('All fields marked with an asterisk (*) are required!!');
                       }
                   );



                  

                } else {
                    commonService.GetString("AllAsteriskRequired").then(function (d) {
                        alertify.error(d.data);
                    },
                        function (d) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                        }
                    );
                }
            }


            $scope.addLogsToProductList = function () {
                var exist = false;
                
                if ($scope.ProductId !== null && $scope.qty !== null && $scope.GradeId !== null && $scope.Length !== null && $scope.Diameter !== null) {
                    var prSize = (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);
                    var addedSize = 0;
                    if ($scope.woodProducts.length > 0) {
                        angular.forEach($scope.woodProducts, function (value, key) {
                            if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Diameter == $scope.Diameter) {
                                addedSize += value.TotalSizeM3;
                            }

                        });
                    }

                    $scope.checkDet = {
                        ProductId: $scope.ProductId,
                        GradeId: $scope.GradeId,
                        Length: $scope.Length,
                        Diameter: $scope.Diameter,
                        Qty: $scope.qty

                    };
                    commonService.CheckAvailability(false, true, false, $scope.StoreId, $scope.checkDet).then(function (d) {

                        if (!d.data.status) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                            return;
                        }

                        if (d.data.total <= 0) {
                            alertify.error('there is not enough product in store');
                            return;
                        }
                        if (d.data.total > 0) {
                           
                            var remain = d.data.total - addedSize;
                            if (remain < prSize) {
                                alertify.error('there is not enough product in store');
                                return;
                            }
                            else
                            {
                                angular.forEach($scope.woodProducts, function (value, key) {
                                    if (value.ProductId == $scope.ProductId && value.GradeId == $scope.GradeId && value.Length == $scope.Length && value.Diameter == $scope.Diameter) {
                                        value.Qty += $scope.qty;
                                        value.OrderedM3 += $scope.OrderedM3;
                                        value.OrderedNo += $scope.OrderedNo;
                                        value.VariationM3 += $scope.VariationM3;
                                        value.VariationNo += $scope.VariationNo;
                                        value.TotalSizeM3 += (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);
                                        exist = true;
                                    }

                                });
                                $scope.TotalSize = (($scope.Diameter / 100) * ($scope.Length / 100) * $scope.qty);

                                if (exist == false) {
                                    pushWoodProductToList();
                                }

                                $scope.totalWoodQty = $scope.sum($scope.woodProducts, "TotalSizeM3");
                                
                                clearInputs();

                            }
                           
                        }
                    },
                       function (d) {
                           alertify.error('All fields marked with an asterisk (*) are required!!');
                       }
                   );


                  

                } else {
                    commonService.GetString("AllAsteriskRequired").then(function (d) {
                        alertify.error(d.data);
                    },
                        function (d) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                        }
                    );
                }
            }

            $scope.addNonWoodProductToList = function () {
                var exist = false;
                var productName = $("#ProductId :selected").text();
                var unitName = $("#UnitId :selected").text();

                if ($scope.ProductId !== null && $scope.qty !== null && $scope.UnitId !== null) {

                    var addedSize = 0;
                    if ($scope.nonWoodProducts.length > 0) {
                        angular.forEach($scope.nonWoodProducts, function (value, key) {
                            if (value.ProductId == $scope.ProductId && value.UnitId == $scope.UnitId) {
                                addedSize += value.Qty;
                            }

                        });
                    }

                    $scope.checkDet = {
                        ProductId: $scope.ProductId
                    };
                    commonService.CheckAvailability(false, false, false, $scope.StoreId, $scope.checkDet).then(function (d) {

                        if (!d.data.status) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                            return;
                        }

                        if (d.data.total <= 0) {
                            alertify.error('there is not enough product in store');
                            return;
                        }
                        if (d.data.total > 0) {

                            var remain = d.data.total - addedSize;
                            if (remain < $scope.qty) {
                                alertify.error('there is not enough product in store');
                                return;
                            }
                            else {
                              
                                angular.forEach($scope.nonWoodProducts, function (value, key) {
                                    if (value.ProductId == $scope.ProductId && value.UnitId == $scope.UnitId) {
                                        value.Qty += $scope.qty;
                                        value.OrderedNo += $scope.OrderedNo;
                                        value.VariationNo += $scope.VariationNo;
                                        exist = true;
                                    }

                                });
                                if (exist == false) {
                                    $scope.nonWoodProducts.push({
                                        SerialNoTypeId: $scope.SerialNoTypeId,
                                        SerialNo: $scope.serialNo,
                                        IssueVoucherNo: $scope.IssueVoucherNo,
                                        Date: $scope.Date,
                                        SupplierId: $scope.SupplierId,
                                        StoreId: $scope.StoreId,
                                        ProdName: productName,
                                        ProductId: $scope.ProductId,
                                        Qty: $scope.qty,
                                        UnitId: $scope.UnitId,
                                        UntName: unitName,
                                        Note: $scope.pnote,

                                    });
                                }
                                $scope.disableStore = true;
                                $scope.totalQty = $scope.sum($scope.nonWoodProducts, "Qty");
                                clearInputs();

                            }

                        }
                    },
                       function (d) {
                           alertify.error('All fields marked with an asterisk (*) are required!!');
                       }
                   );


                } else {
                    commonService.GetString("AllAsteriskRequired").then(function (d) {
                        alertify.error(d.data);
                    },
                        function (d) {
                            alertify.error('All fields marked with an asterisk (*) are required!!');
                        }
                    );
                }
            }

            $scope.selectFileForUpload = function (files) {
                $scope.selectedFiles.splice(0, $scope.selectedFiles.length);
                $scope.$apply(function () {
                    for (var i = 0; i < files.length; i++) {
                        $scope.selectedFiles.push(files[i]);
                    }
                });
               
                $scope.hasImage = true;
            };
            
            $scope.sum = function (items, prop) {
                return items.reduce(function (a, b) {
                    return a + b[prop];
                }, 0);
            };

            $scope.removeNonWoodItem = function (item) {
                $scope.nonWoodProducts.splice($scope.nonWoodProducts.indexOf(item), 1);
                $scope.totalQty = $scope.sum($scope.nonWoodProducts, "Qty");
            };
            $scope.removeWoodItem = function (item) {
                $scope.woodProducts.splice($scope.woodProducts.indexOf(item), 1);
                $scope.totalWoodQty = $scope.sum($scope.woodProducts, "TotalSizeM3");

            };
             function clearList() {
                $scope.nonWoodProducts.splice(0,$scope.nonWoodProducts.length);
                $scope.woodProducts.splice(0, $scope.woodProducts.length);
                $scope.totalQty = 0;
                $scope.totalWoodQty = 0;
             };
            
            function clearInputs() {
                $scope.ProductId = null;
                $scope.qty = null;
                $scope.UnitId = null;
                $scope.GradeId = null;
                $scope.Length = null;
                $scope.Width = null;
                $scope.Thickness = null;
                $scope.DryingId = null;
                $scope.Steaming = null;
                $scope.Diameter = null;
                $scope.pnote= null;
                $scope.TotalSize= null;
                $scope.OrderedM3= null;
                $scope.OrderedNo = null;
            };
            function clearServerList() {
                $scope.productsDetails.splice(0, $scope.productsDetails.length);
                $scope.receivedProduct = {};
            };
            $scope.SaveAll = function () {
                $scope.clicked = true;

                if ($scope.SerialNoTypeId !== null &&
                    $scope.serialNo !== null &&
                    $scope.IssueVoucherNo !== null && $scope.IssueTypeId !== null && $scope.Receiver !== null && $scope.RecipientName !== null &&
                    $scope.Date !== null && $scope.StoreId !== null && ($scope.nonWoodProducts.length > 0 || $scope.woodProducts.length > 0)) {

                    populateLists();
                    productService.IssueProducts($scope.issuedProduct, $scope.productsDetails).then(function (d) {
                        if (d.data.status === true) {
                            if (d.data.id !== null && $scope.hasImage) {
                                productService.UploadDocs($scope.selectedFiles, d.data.id).then(function (d) {
                                    if (d.status === true) {
                                        window.location.href = "/Store/ProductIssue";
                                    }
                                },
                          function (d) {
                              $scope.clicked = false;

                              alertify.error('The Record has been saved but you have Error when uploading documents!!');
                          });
                               
                            } else {
                                window.location.href = "/Store/ProductIssue";
                            }
                           

                        } else {
                            $scope.clicked = false;

                            alertify.error(d.data.msg);
                            clearServerList();

                        }
                    });
                  
                } else {
                    $scope.clicked = false;

                    commonService.GetString("AllAsteriskRequired")
                      .then(function (d) {
                          alertify.error(d.data);
                      },
                          function (d) {
                              alertify.error('All fields marked with an asterisk (*) are required!!');
                          }
                      );
                    return;
                }

            };

            
           
            function populateLists() {
                if ($scope.nonWoodProducts.length > 0) {
                    angular.forEach($scope.nonWoodProducts, function (value, key) {
                        if (value.ProductId != null && value.UnitId != null && value.Qty != null) {
                            $scope.productsDetails.push({
                                ProductId: parseInt(value.ProductId),
                                Qty: value.Qty,
                                GradeId: null,
                                Diameter: null,
                                Length: null,
                                Width: null,
                                Thickness: null,
                                DryingId: null,
                                Steaming: null,
                                UnitId: parseInt(value.UnitId),
                                OrderedM3: null,
                                OrderedNo: value.OrderedNo,
                                Note: value.Note,
                                TotalSizeM3: null

                            });
                        }

                    });
                }


                if ($scope.woodProducts.length > 0) {
                    angular.forEach($scope.woodProducts, function (value, key) {
                        if (value.ProductId != null && value.GradeId != null && value.Qty != null && value.Length != null) {
                            $scope.productsDetails.push({
                                ProductId: parseInt(value.ProductId),
                                Qty: value.Qty,
                                GradeId: parseInt(value.GradeId),
                                Diameter: parseInt(value.Diameter),
                                Length: parseInt(value.Length),
                                Width: parseInt(value.Width),
                                Thickness: parseInt(value.Thickness),
                                DryingId: parseInt(value.DryingId),
                                Steaming: value.Steaming,
                                UnitId: null,
                                OrderedM3: value.OrderedM3,
                                OrderedNo: value.OrderedNo,
                                Note: value.Note,
                                TotalSizeM3: value.TotalSizeM3

                            });
                        }

                    });
                }

                $scope.Date = $("#Date").val();
              
                $scope.issuedProduct = {
                    SerialNoTypeId: parseInt($scope.SerialNoTypeId),
                    SerialNo:$scope.serialNo,
                    IssueVoucherNo: $scope.IssueVoucherNo,
                    RecipientName: $scope.RecipientName,
                    Date: $scope.Date,
                    Receiver: $scope.Receiver,
                    StoreId: parseInt($scope.StoreId),
                    IssueTypeId: $scope.IssueTypeId,
                    Note: $scope.vnote

                };
               
            }

         



        }
    ]);