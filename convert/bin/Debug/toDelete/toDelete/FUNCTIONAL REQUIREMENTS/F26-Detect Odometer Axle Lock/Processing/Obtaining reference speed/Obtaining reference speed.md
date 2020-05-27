
[iTC_CC_ATP-SwRS-0128]
ReferenceSpeedUnderThreshold_1，来自CCNV的参考速度1是否小于指定阈值。
ReferenceSpeedUnderThreshold_1 defines whether the referenced speed 1 from CCNV is lower than a configurable threshold.
```
	def ReferenceSpeedUnderThreshold_1(k):
	    if RadarSpeedValid(k):
	        return (RadarRawSpeed(k) < ATPsetting.OdoLockedAxleThresholdSpeed)
	    else:
	        return (ATOcontrolTimeValid(k)
	                and NonVitalRequest.OdometerRef1SpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1044], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0129]
ReferenceSpeedAvailable_1，来自CCNV的参考速度1是否可用
ReferenceSpeedAvailable_1 defines whether the referenced speed 1 from CCNV is valid or not. 
```
	def ReferenceSpeedAvailable_1(k):
	    return (RadarSpeedValid(k)
	            or (ATOcontrolTimeValid(k)
	                and NonVitalRequest.OdometerRef1Available(k)))
```
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1044], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0130]
ReferenceSpeedUnderThreshold_2，来自CCNV的参考速度2是否小于指定阈值。
ReferenceSpeedUnderThreshold_2 defines whether the referenced speed 2 from CCNV is lower than a configurable threshold. 
```
	if (ATOcontrolTimeValid(k) == True)
	    ReferenceSpeedUnderThreshold_2(k) = NonVitalRequest.OdometerRef2SpeedUnderThreshold(k)
	else:
	    ReferenceSpeedUnderThreshold_2 = False
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1044], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0131]
ReferenceSpeedAvailable_2，来自CCNV的参考速度2是否可用
ReferenceSpeedAvailable_2 shows whether the referenced speed 2 from CCNV is effective or not. 
```
	if (ATOcontrolTimeValid(k) == True)
	    ReferenceSpeedAvailable_2(k) = NonVitalRequest.OdometerRef2Available(k)
	else:
	    ReferenceSpeedAvailable_2 = False
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1044], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0231]
OdometerRef_1.PossiblyDisabled，当本端里程计可用且不为0速，而参考速度1可用但为0速时，则认为参考速度1可能错误
The independent source of odometry reference 1 said to disable if following conditions reached:
local source of odometry is available (ValidWheelKinematic),
and OdometerSpeedUnderThreshold indicates that train speed is greater than reference speed threshold,
and source of odometry reference 1 is available,
and odometer reference 1 indicates that train speed is less than reference speed threshold.
```
	OdometerRef_1. PossiblyDisabled(k)
	   = (ReferenceSpeedAvailable_1(k)
	      and ReferenceSpeedUnderThreshold_1(k)
	      and OdometerSpeedAvailable(k)
	      and not OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0232]
OdometerRef_2. PossiblyDisabled，当本端里程计可用且不为0速，而参考速度2可用但为0速时，则认为参考速度2可能错误
The independent source of odometry reference 2 said to disable if following conditions reached:
local source of odometry is available (ValidWheelKinematic),
and OdometerSpeedUnderThreshold indicates that train speed is greater than reference speed threshold,
and source of odometry reference 2 is available,
and odometer reference 2 indicates that train speed is less than reference speed threshold.
```
	OdometerRef_2. PossiblyDisabled(k)
	   = (ReferenceSpeedAvailable_2(k)
	      and ReferenceSpeedUnderThreshold_2(k)
	      and OdometerSpeedAvailable(k)
	      and not OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0233]
OdometerRef_1.PossiblyEnabled，当本端里程计和参考速度1均可用且测得列车在动时，认为参考速度1可能已恢复有效。
The independent source of odometry reference 1 said to enable if following conditions reached:
local source of odometry is available,
and OdometerSpeedUnderThreshold indicates that train speed is greater than reference speed threshold,
and source of odometry reference 1 is available,
and odometer reference 1 indicates that train speed is greater than reference speed threshold.
```
	OdometerRef_1. PossiblyEnabled(k)
	  = (ReferenceSpeedAvailable_1(k)
	      and not ReferenceSpeedUnderThreshold_1(k)
	      and OdometerSpeedAvailable(k)
	      and not OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0234]
OdometerRef_2.PossiblyEnabled，当本端里程计和参考速度2均可用且测得列车在动时，认为参考速度2可能已恢复有效。
The independent source of odometry reference 2 said to enable if following conditions reached:
local source of odometry is available,
and OdometerSpeedUnderThreshold indicates that train speed is greater than reference speed threshold,
and source of odometry reference 2 is available,
and odometer reference 2 indicates that train speed is greater than reference speed threshold.
```
	OdometerRef_2. PossiblyEnabled(k)
	  = (ReferenceSpeedAvailable_2(k)
	     and not ReferenceSpeedUnderThreshold_2(k)
	     and OdometerSpeedAvailable(k)
	     and not OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0235]
OdometerRef_1.OutOfOrder，当判断参考速度1可能不可用时，延迟一段时间，若仍不可用，则判断参考速度1失效。
The independent source of odometry reference 1 is said to be out of order if it is possibly disabled during more than ATPsetting.OdoLockedAxleDisablingLatency.
```
	if (OdometerRef_1.PossiblyDisabled(k) == True
	    and OdometerRef_1.PossiblyDisabled(k-1) == True
	    ...
	    and OdometerRef_1.PossiblyDisabled(k+1-ATPsetting.OdoLockedAxleDisablingLatency) == True)
	    OdometerRef_1.OutOfOrder  = True
```
当判断参考速度1可能可用时，延迟一段时间，若仍可用，则判断参考速度1有效
When the independent source of odometry reference 1 had out of order, it considered not out of order one if the source of odometry reference 1 is possibly enabled during more than ATPsetting.OdoLockedAxleEnablingLatency:
```
	if (OdometerRef_1.PossiblyEnabled(k) == True
	    and OdometerRef_1.PossiblyEnabled(k-1) == True
	    ...
	    and OdometerRef_1.PossiblyEnabled(k+1-ATPsetting.OdoLockedAxleEnablingLatency) == True)
	    OdometerRef_1.OutOfOrder = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0136], [iTC_CC_ATP_SwHA-0237]
[End]
[iTC_CC_ATP-SwRS-0236]
OdometerRef_2.OutOfOrder，当判断参考速度2可能不可用时，延迟一段时间，若仍不可用，则判断参考速度2失效。
The independent source of odometry reference 2 is said to be out of order if it is possibly disabled during more than ATPsetting.OdoLockedAxleDisablingLatency.
```
	if (OdometerRef_2.PossiblyDisabled(k) == True
	     and OdometerRef_2.PossiblyDisabled(k-1) == True
	     ...
	     and OdometerRef_2.PossiblyDisabled(k+1-ATPsetting.OdoLockedAxleDisablingLatency) == True)
	    OdometerRef_2.OutOfOrder = True
```
当判断参考速度2可能可用时，延迟一段时间，若仍可用，则判断参考速度2有效。
When the independent source of odometry reference 2 had out of order, It considered not out of order one if the source of odometry reference 2 is possibly enabled during more than ATPsetting.OdoLockedAxleEnablingLatency:
```
	if (OdometerRef_2.PossiblyEnabled(k) == True
	    and OdometerRef_2.PossiblyEnabled(k-1) == True
	    ...
	    and OdometerRef_2.PossiblyEnabled(k+1-ATPsetting.OdoLockedAxleEnablingLatency) == True)
	   OdometerRef_2.OutOfOrder = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0136], [iTC_CC_ATP_SwHA-0237]
[End]
[iTC_CC_ATP-SwRS-0237]
OdometerRef_1.Contradictory，若参考速度1有效且判断车动，而本端里程计判断车静止，则认为参考速度1判断出里程计可能故障。
The source of odometry reference 1 said to be contradictory with local source of odometry if:
local source of odometry is available (ValidWheelKinematic)
and OdometerSpeedUnderThreshold indicates that wheel speed is less than reference speed threshold,
and source of odometry reference 1 is available and not out of order,
and odometer reference 1 indicates that train speed is greater than reference speed threshold.
```
	OdometerRef_1. Contradictory(k)
	 = (not OdometerRef_1.OutOfOrder(k)
	     and ReferenceSpeedAvailable_1(k)
	     and not ReferenceSpeedUnderThreshold_1(k)
	     and ValidWheelKinematic(k)
	     and OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1156], [iTC_CC_ATP_SwHA-0091]
[End]
[iTC_CC_ATP-SwRS-0238]
OdometerRef_2. Contradictory，若参考速度2有效且判断车动，而本端里程计判断车静止，则认为参考速度2判断出里程计可能故障。
The source of odometry reference 2 said to be contradictory with local source of odometry if:
local source of odometry is available (ValidWheelKinematic)
and OdometerSpeedUnderThreshold indicates that wheel speed is less than reference speed threshold,
and source of odometry reference 2 is available and not out of order,
and odometer reference 2 indicates that train speed is greater than reference speed threshold.
```
	OdometerRef_2.Contradictory (k)
	 = (not OdometerRef_2.OutOfOrder(k)
	     and ReferenceSpeedAvailable_2(k)
	     and not ReferenceSpeedUnderThreshold_2(k)
	     and ValidWheelKinematic(k)
	     and OdometerSpeedUnderThreshold(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1156], [iTC_CC_ATP_SwHA-0091]
[End]
