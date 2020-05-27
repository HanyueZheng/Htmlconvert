
ATP控制车辆是否输出停车制动，出于安全考虑，在要求输出停车制动时将该输出端口设置为限制状态，不输出停车制动时将该端口置为允许状态。
ATP shall control the parking brake of the train. Due to the safety oriented, ATP shall set the output port as restricted status when outputting the parking brake order, and set the port as permissible status when the ATP does not send parking brake.
[iTC_CC_ATP-SwRS-0133]
PBforOperationalRequest，来自CCNV的ZVRD输出请求
PBforOperationalRequest stands for the ZVRD output order from CCNV.
```
	if (ATOcontrolTimeValid(k) == True)
	    PBforOperationalRequest(k)
	       = not NonVitalRequest.VitalParkingBrakingNotRequested(k)
	else:
	     PBforOperationalRequest = True
```
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0068], [iTC_CC-SyAD-1044], [iTC_CC-SyAD-0231]
[End]
[iTC_CC_ATP-SwRS-0359]
TrainParkingBrakeRequested，判断本周期是否需要施加停车制动。条件如下：
由于moral time 导致需要输出停车制动；
或者，由于超能导致需要输出停车制动；
或者，由于在PSD区域内车门未关闭而导致需要输出停车制动；
或者，由于NUDE导致需要输出停车制动；
或者，CCNV请求需要输出停车制动；
或者，由于PSD未关闭而导致需要输出停车制动
或者，本周期已请求EB输出。
TrainParkingBrakeRequested, determine whether to apply parking brake. This variable shall be True when one of the following conditions met:
Train is in front of a possibly non-exclusive route,
Synthesis of speed constraints on the train implies that it is not allowed to move anymore. Any movement may lead to an hazardous situation,
Train is located on a passenger exchange area with PSD and train doors are not proven closed and locked,
Train is located on a passenger exchange area with PSD and PSD are not proven closed and locked,
There is a possibility of undetectable dangers,
An operational parking brake is requested,
The PSD are opened and are under the supervision of ATP,
The EB has been requested in this cycle.
```
	TrainParkingBrakeRequested = (PBonNonExclusiveRoute(k)
	                                    or PBforOverEnergy(k)
	                                    or PBforTrainDoorsNotClosedAndLocked(k)
	                                    or PBforPSDnotClosedAndLocked(k)
	                                    or PBforUndetectableDangerRisk(k)
	                                    or PBforOperationalRequest(k)
	                                    or PBforPSDopenedAndSupervisedByATP(k)
	                                    or not InhibitEmergencyBrake(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230], [iTC_CC_ATP_SwHA-0153]
[End]
[iTC_CC_ATP-SwRS-0360]
InhibitParkingBrake，当前不施加停车制动。
InhibitParkingBrake，ATP software do not apply the parking brake.
```
	InhibitParkingBrake = not TrainParkingBrakeRequested(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230]
[End]
