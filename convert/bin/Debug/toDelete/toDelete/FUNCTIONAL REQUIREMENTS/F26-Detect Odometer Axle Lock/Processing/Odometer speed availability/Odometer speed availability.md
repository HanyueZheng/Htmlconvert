
[iTC_CC_ATP-SwRS-0637]
OdometerSpeedAvailable，当前里程计测速是否可用于参考速度判断
```
	OdometerSpeedAvailable(k):
	    return (ValidWheelKinematic(k)
	            and OdometerState(k) is INITIALIZED)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153]
[End]
[iTC_CC_ATP-SwRS-0230]
OdometerSpeedUnderThreshold，本端里程计测速低于阈值。
ATP shall detect whether the measured wheel speed is under threshold.
```
	def OdometerSpeedUnderThreshold(k):
	    return (WheelMinSpeed(k) < ATPsetting.OdoLockedAxleThresholdSpeed)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1153], [iTC_CC-SyAD-1155]
[End]
