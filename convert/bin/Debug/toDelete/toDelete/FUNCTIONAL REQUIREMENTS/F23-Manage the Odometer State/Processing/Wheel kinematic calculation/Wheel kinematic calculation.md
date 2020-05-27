
[iTC_CC_ATP-SwRS-0170]
SensorTestContradiction，当里程计读数为0，但中断中却未进行传感器测试时，设置该变量为True，否则为False。
NoCommunicationWithOdometer，当SensorTestContradiction保持为True超过限定时间后，设置该值为真，表明中断中的传感器测试判断失败。
ATP shall invalidate wheel kinematic if minimum odometer motion is null and sensors test is not performed for more than ATPsetting.OdoTestContradictionDuration.
```
	def SensorTestContradiction(k):
	    return (not WheelFilteredStopped(k)
	            and TeethCounter(k-1) == TeethCounter(k-2)
	            and not SensorTestPerformed(k))
```
```
	def NoCommunicationWithOdometer(k):
	    if (Initialization
	        or not SensorTestContradiction(k)):
	        SensorTestContradictionDuration = 0
	        return False
	    else:
	        SensorTestContradictionDuration = SensorTestContradictionDuration(k-1) + 1
	        
	        if (SensorTestContradictionDuration > ATPsetting.OdoTestContradictionDuration):
	            return True
	        else:
	            return False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1161], [iTC_CC_ATP_SwHA-0066]
[End]
[iTC_CC_ATP-SwRS-0190]
ValidWheelKinematic，车轮运动学特性有效.
Wheel kinematic is valid if odometer is valid, the calculated motion is not greater than the default value, and there is communication with odometer.
```
	ValidWheelKinematic(k)
	 = ((OdometerState(k) != INVALID)
	   and (not WheelKinematicsInvalidForCogCount(k))
	   and (not NoCommunicationWithOdometer(k)))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0186], [iTC_CC-SyAD-1161], [iTC_CC-SyAD-0960]
[End]
[iTC_CC_ATP-SwRS-0636]
WheelMinSpeed，里程计测得车轮最小速度，非负值。
```
	def WheelMinSpeed(k):
	    return round.floor(abs(WheelMinimumMovement(k)) / ATP_CYCLE_TIME)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136]
[End]
[iTC_CC_ATP-SwRS-0204]
WheelMaxSpeed，ATP根据里程计测得位移计算车轮最大速度，该值为非负数，并且向上取整。
ATP calculates the maximum wheel speed according to the maximum wheel movement; this value is non-negative and rounded up.
```
	def WheelMaxSpeed(k):
	    return round.ceil(abs(WheelMaximumMovement(k)) / ATP_CYCLE_TIME)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0146]
[End]
