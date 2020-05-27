
[iTC_CC_ATP-SwRS-0690]
ZoneVSLNotExceedTrainSpeedLimit，ATP应始终将项目配置的限速值为ATPsetting.MPauthLimitSpeed作为安全速度限制区域。
限制区能量
```
	def ZoneVSLNotExceedTrainSpeedLimit(k):
	    return (TrainEnergy(k) < pow(ATPsetting.MPauthLimitSpeed))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0276], [iTC_CC-SyAD-0300], [iTC_CC_ATP_SwHA-0264]
[End]
