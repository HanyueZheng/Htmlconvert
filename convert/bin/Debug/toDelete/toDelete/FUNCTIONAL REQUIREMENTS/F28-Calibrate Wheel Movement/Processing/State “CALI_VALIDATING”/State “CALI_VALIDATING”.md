﻿
[iTC_CC_ATP-SwRS-0196]
当发生以下情况时，认为校准失败，从CALI_VALIDATING回到CALI_WAITING
If ATP is in the state of calibration validation in progress and following conditions fulfilled:
train kinematic has been detected not valid,
or excessive slip/slide effect has been detected,
or WheelMinimumMovement sign is in the opposite direction of thus observed on first beacon or becomes null,
or an unexpected beacon has been received. That is, a beacon not belonging calibration validation couple.
In such case, then ATP shall consider that calibration process as not sable and so back to CALI_WAITING.
```
	if ((CalibrationState(k-1) =  CALI_VALIDATING)
	     and ((ValidTrainKinematic(k) == False)
	           or (OdometerState(k) is INVALID)
	           or (SlipSlideDetected(k) == True)
	           or ((CalibrationEnd1RunningForward(k-1) != End1RunningForward(k))
	           or (TrainFilteredStopped(k) == True)
	           or ((NewBeaconObtained(k) == True)
	               and (TrackMap.IsCalibrationValidationBeacon(BeaconMessage.Id(k),
	                                                      CalibrationValidationStartBeacon(k)) == False))))
	    CalibrationState = CALI_WAITING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0145]
[End]
[iTC_CC_ATP-SwRS-0197]
当读到有效的验证信标，并判断之前测得齿距在有效范围内时，认为校准成功，转入CALI_COMPLETED状态；
否则，校准失败，返回CALI_WATING状态。
If ATP is in the state of calibration validation in progress and following conditions fulfilled:
a valid beacon has been received and this beacon is one of possible confirmation beacons related to second beacon signaled of calibration measurement,
and train kinematic was valid,
and no excessive slip/slide effect was detected,
and sign of train motion is still identical to thus detected on first beacon signaling,
and WheelMinimumMovement is not null.
Then,
if resulting calibration range fully includes the calibration range in track map, then ATP shall:
update ATP minimum and maximum calibration with last calibration computed on beacons,
and shall consider that calibration process is CALI_COMPLETED.
else: ATP shall consider that calibration process is not usable and so back to CALI_WATING waiting for new measurement calibration beacons.
```
	if (CalibrationState(k-1) =  CALI_VALIDATING)
	    and (NewBeaconObtained(k) == True)
	    and (TrackMap.IsCalibrationValidationBeacon(BeaconMessage.Id(k),
	                                                        CalibrationValidationStartBeacon(k)))
	    and (SlipSlideDetected(k) == False)
	    and (CalibrationEnd1RunningForward(k-1) == End1RunningForward(k)))
	    if ((TrackMap.CalibrationCoupleMaxDistance(BeaconMessage.Id(k),
	                              CalibrationValidationStartBeacon(k)) <= MaxDistanceRanForValidation)
	        and TrackMap.CalibrationCoupleMinDistance(BeaconMessage.Id(k),
	                             CalibrationValidationStartBeacon(k)) >= MinDistanceRanForValidation)
	         MinCogCalibration = CalibrationResultMin(k)
	         MaxCogCalibration = CalibrationResultMax(k)
	         CalibrationState = CALI_COMPLETED
	    else:
	         CalibrationState = CALI_WAITING
```
Where:
```
	MaxDistanceRanForValidation
	 =(abs(CalibrationValidationStartPositionMin(k)— CogPositionAfterTopLoc(k)) + 1)
	   * CalibrationResultMax(k)
	MinDistanceRanForValidation
	 =(abs(CalibrationValidationStartPositionMax(k)— CogPositionBeforeTopLoc(k)) - 1)
	   * CalibrationResultMin(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0132], [iTC_CC-SyAD-0141], [iTC_CC-SyAD-0142], [iTC_CC-SyAD-0145], [iTC_CC_ATP_SwHA-0069], [iTC_CC-SyAD-0183]
[End]
