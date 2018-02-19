/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
"use strict";
var mClick = 0;
var distX = 0;
var distY =0;
var sum = 0;
var value = 1;
var isDone = false;
//controller 2 for interaction with the slider
var myModule = angular.module('myApp', ["CSS450Slider"]);
myModule.controller("Ctrl2", function ($scope) {
    $scope.mValue_1 = "1";   // this is a tex!
    $scope.mSimpleModel = "20";
    
    $scope.changeSlider = function(number){
        $scope.mValue_1 = number;
        value = $scope.mValue_1;
        document.getElementById("scaler").value = sum*value;
    }; 
    
    
});

var app = angular.module('myApp');
            app.controller('myCtrl', function($scope, $interval) {
            var mCount = 0;
            var mRun;
            $scope.mEcho = "0";
            $scope.timerService = function() {
                if(isDone === false){          //option to stop timer when end measurement 
                    mCount++;
                    $scope.mEcho = mCount;
                }
            };
            
            $scope.start = function() {
                if(document.getElementById("startcheck").checked === true){
                    if (angular.isDefined(mRun))
                        return;
                    $scope.radioSection = "pixels";
                    mRun = $interval($scope.timerService, 1000);
                    if(mCount === 0){
                         document.getElementById("statustext").value = "Measuring in progress: before first click ";
                    } 
                    
                    $scope.mEcho = "ON"; // Date.now();
                 } else {         //when disabling the session all values reset
                    if (angular.isDefined(mRun)) {
                        $interval.cancel(mRun);
                        mRun = undefined;
                        mCount=0;
                        mClick=0;
                        sum=0;
                        distX=0;
                        distY=0;
                        value=0;
                        isDone=false;
                        document.getElementById("totalPix").value = "0";
                        document.getElementById("scaler").value = "0";
                        document.getElementById("statustext").value = "Measurement Disabled ";
                        $scope.mEcho = "0"; // Date.now();
                        
                    }             
                 }
            };
            
            
            $scope.serviceMouse = function($event) {
                //movement pixels coordenants 
                $scope.x = $event.x;
                $scope.y = $event.y;
                
            };  
            
            $scope.serviceDown = function($event) {
                console.log($event);
                switch($event.which) {
                    case 1:
                        isDone = false;
                   // if(isDone === false) {   
                        mClick++;
                        if(mClick >= 2) {
                            sum = sum + Math.round(Math.sqrt((($event.x-distX)*($event.x-distX) + ($event.y-distY)*($event.y-distY))));   
                        } 
                
                    document.getElementById("totalPix").value = sum;
                    document.getElementById("scaler").value = sum*value;
                    distX = $event.x;
                    distY = $event.y;
                    $scope.x2 = $event.x;
                    $scope.y2 = $event.y;
                
                    document.getElementById("statustext").value = "Measuring in progress: Number of LMB clicks: " + mClick;
               // }
                    break;
                 case 3:
                    document.getElementById("statustext").value = "Measuring finished: total Number of LMB clicks: " + mClick;
                    isDone = true;
                    break;
             } 
            };
            
    });