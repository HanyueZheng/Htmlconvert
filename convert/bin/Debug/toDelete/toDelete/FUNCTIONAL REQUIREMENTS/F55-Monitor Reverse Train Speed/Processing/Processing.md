
当列车在RMR模式下向激活车头的反向运行时，ATP需监控列车速度不能超过项目配置的反向运行最大速度，如果超过该速度，则触发EB。如果列车已RMR反向运行超过项目配置的最大距离，则ATP保持输出EB，禁止列车继续已RMR模式反向运行。
When the train selected on RMR mode, the train moved reversely related to the active cab. ATP shall request EB if the speed of the reverse is greater than the project limits. If the train reversing distance is greater than the project limits, ATP shall keep requesting EB to inhibit train moving on RMR mode.
[iTC_CC_ATP-SwRS-0759]
LongDistanceReverseAuthorized，长距离倒车模式是否授权，其状态来自于项目可配置的列车输入采集。
LongDistanceReverseAuthorized represents the authorization of long distance reverse. 
```
	def LongDistanceReverseAuthorized(k):
	    return Offline.GetLongDistanceReverseAuthorized(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1394], [iTC_CC_ATP_SwHA-0278],
 [iTC_CC-SyAD-1442][End]
NOTES：
对于在指定区域内“长距离倒车”功能的监控，是通过离线数据配置，由需求SwRS-0717和0743实现的：
在项目的线路地图中：
在需要进行长距离倒车的区域配置VitalZone及其相应的PermissiveZoneLogicalInput属性，不需要变量。ATP保证 当列车定位完全在该区域内 时，设置 PermissiveZoneLogicalInput为True
在项目的安全配置参数中：
配置LongDistanceReverseAuthorized为上述PermissiveZoneLogicalInput与CCNV授权长距离倒车（即CCNV判断过停）同时为True，此时ATP不再监控普通的RMR倒车距离，也不监控Rollback；
在TrainType/MotionProtection/inhibition/LimitSpeed中，配置一个选择RMR模式且 LongDistanceReverseAuthorized为True的限速，作为长距离RMR的限速。ATP保证在该条件满足时，以该限速监控列车。
ATP对于Rollback，RMR，长距离倒车（LDR）三种距离的累加条件如表所示：
Table 511 Backward distance account rules

||LongDistanceReverseAuthorized|not LongDistanceReverseAuthorized|
|-----|
|RMRselectedDrivingMode|RMR向前溜车：不抵消RMR距离，不抵消Rollback距离|RMR向前溜车：抵消RMR距离，不抵消Rollback距离|
||长距离倒车：不累加RMR距离，不累加Rollback距离|普通RMR倒车：累加RMR距离，不累加Rollback距离|
|not RMRselectedDrivingMode|LDR区域向前：不抵消RMR距离，抵消Rollback距离|正常向前：抵消RMR距离，抵消Rollback距离|
||LDR区域回溜：不累加RMR距离，累加Rollback距离|普通回溜：不累加RMR距离，累加Rollback距离|

[iTC_CC_ATP-SwRS-0305]
ReverseDistanceAccount_1，累加RMR模式下的倒车距离（负值表示倒车）：
初始化时设置该值为0；
否则，如果列车运动学无效，则设置为配置参数的默认值；
否则，在END_1激活且非长距离倒车授权的前提下：
若里程计已初始化，且列车向END_1方向运行，则减小倒车距离绝对值，大于零则等于0
否则，如果里程计已初始化，且选择RMR模式，则累加倒车距离
否则，即里程计还未初始化，则保持距离不变。
其他情况，保持累计距离不变。
When train front extremity is END_1 and traction effort is supposed to be in the opposite direction of travel, ReverseDistanceAccount_1 is the estimated maximum distance which separates current front extremity 1 position to last most forward position reached by this extremity. ATP shall evaluate ReverseDistanceAccount_1 in order to control that speed does not exceed reverse speed limit function.
```
	def ReverseDistanceAccount_1(k):
	    if (Initialization)
	        return 0
	    elif (not ValidTrainKinematic(k)):
	        return ATPsetting.ReverseDistWithoutMotionAvailable
	    elif (TrainFrontEnd is END_1
	           and not LongDistanceReverseAuthorized(k)):
	        if (OdometerState(k) is INITIALIZED):
	            if (End1RunningForward(k)):
	                return min(0, ReverseDistanceAccount_1(k-1) + MinimumTrainMotion(k))
	            elif (RMRselectedDrivingMode(k)):
	                return (ReverseDistanceAccount_1(k-1) + MaximumTrainMotion(k))
	            else:
	                return ReverseDistanceAccount_1(k-1)
	        else:
	            return ReverseDistanceAccount_1(k-1)
	    else:
	        return ReverseDistanceAccount_1(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0331], [iTC_CC-SyAD-1308]
[End]
[iTC_CC_ATP-SwRS-0306]
ReverseDistanceAccount_2，累加RMR模式下的倒车距离（负值表示倒车）：
初始化时设置该值为0；
否则，如果列车运动学无效，则设置为配置参数的默认值；
否则，在END_2激活且非长距离倒车授权的前提下：
如果里程计已初始化，且列车向END_2方向运行，则减小倒车距离绝对值，大于零则等于0
否则，如果里程计已初始化，且选择RMR模式时累加倒车距离
否则，即里程计未初始化，则保持距离不变；
其他情况，保持累计距离不变。
When train front extremity is END_2 and traction effort is supposed to be in the opposite direction of travel, ReverseDistanceAccount_2 is the estimated maximum distance which separates current front extremity 2 position to last most forward position reached by this extremity. ATP shall evaluate ReverseDistanceAccount_2 in order to control that speed does not exceed ReverseSpeedRestrictions reverse speed limit function.
```
	def ReverseDistanceAccount_2(k):
	    if (Initialization)
	        return 0
	    elif (not ValidTrainKinematic(k)):
	        return ATPsetting.ReverseDistWithoutMotionAvailable
	    elif (TrainFrontEnd(k) is END_2
	           and not LongDistanceReverseAuthorized(k)):
	        if (OdometerState(k) is INITIALIZED):
	            if (End2RunningForward(k))
	                return min(0, ReverseDistanceAccount_2(k-1) - MinimumTrainMotion(k))
	            elif (RMRselectedDrivingMode(k)):
	                return (ReverseDistanceAccount_2(k-1) — MaximumTrainMotion(k))
	            else:
	                return ReverseDistanceAccount_2(k-1)
	        else:
	            return ReverseDistanceAccount_2(k-1)
	    else:
	        return ReverseDistanceAccount_2(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0331], [iTC_CC-SyAD-1308]
[End]
[iTC_CC_ATP-SwRS-0753]
ReverseSpeedRestrictions，根据当前计算的倒车累加距离在ATPsetting.ReverseLimit数组中索引的当前最大允许倒车速度。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0330]
[End]
[iTC_CC_ATP-SwRS-0307]
ReverseOverSpeed，超过RMR模式限速的条件：
ReverseOverSpeed shall be True if following conditions fulfilled:
driving selector indicates that traction effort is supposed to be in the opposite direction of travel,
train front extremity is END_2 or END_1,
and movement observed is the opposite direction of travel,
and:
over-estimated train speed is greater than reverse speed restrictions currently applicable for this direction of travel,
or else: if reverse speed restrictions currently applicable is null for this direction of travel,
Or else: train kinematic is invalid.
```
	def ReverseOverSpeed(k):
	    if (not RMRselectedDrivingMode(k)
	         or LongDistanceReverseAuthorized(k)):
	        return False
	    elif (not ValidTrainKinematic(k)):
	        return True
	    else:
	        return ((TrainFrontEnd(k) is END_2):
	                   and ((End1RunningForward(k) and not End2RunningForward(k)
	                          and (TrainMaxSpeed(k)
	                                > ReverseSpeedRestrictions(ReverseDistanceAccount_2(k))))
	                        or (ReverseSpeedRestrictions(ReverseDistanceAccount_2(k)) == 0)))
	                  or (TrainFrontEnd(k) is END_1
	                       and ((End2RunningForward(k) and not End1RunningForward(k)
	                  and (TrainMaxSpeed(k) > ReverseSpeedRestrictions(ReverseDistanceAccount_1(k))))
	                             or (ReverseSpeedRestrictions(ReverseDistanceAccount_1(k)) == 0))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0330], [iTC_CC-SyAD-0279], [iTC_CC_ATP_SwHA-0125], [iTC_CC-SyAD-1308]
[End]
[iTC_CC_ATP-SwRS-0308]
EBforReverseOverSpeed，由于RMR下倒车超速而导致EB
ATP shall request emergency braking if a reverse speed limit is over-run for unwilling rollback or excessive reverse motion.
```
	EBforReverseOverSpeed = ReverseOverSpeed(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0331], [iTC_CC-SyAD-0279], [iTC_CC-SyAD-1308], [iTC_CC_ATP_SwHA-0126]
[End]
