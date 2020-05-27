
[iTC_CC_ATP-SwRS-0401]
TrainHeadOrientation，ATP需将车头最小定位的运营方向作为列车运营方向发送给ZC。规则见SwRS-0403。
ATP shall send the orientation of minimum location of current active cab id to the ZC. For the rule can refer to SwRS-0403.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0403]
TrainHeadMinLocation，车头最小定位位置。
根据[REF5]，在LocReport中的坐标单位为0.5米，因此需进行单位转换。转换时应当向列车的“后方”即上游方向取整。
如果列车失位，则设置相关定位信息为无效值；
否则，如果列车向UP方向运行，则：
车头最小定位所在BLOCK号不变；
车头最小定位所在坐标以0.5米为单位向下取整；
车头方向为LOCREPORT_DIRECTION_UP。
否则，如果车头最小定位坐标加0.5米小于该BLOCK长度，则：
车头最小定位所在BLOCK号不变；
车头最小定位所在坐标以0.5米为单位向上取整；
车头方向为LOCREPORT_DIRECTION_DOWN。
否则，如果车头最小定位所在BLOCK，与该BLOCK的UP方向下个BLOCK之间存在灯泡线极点，则：
车头最小定位所在BLOCK需改为其UP方向的下个BLOCK；
车头最小定位所在坐标为下个BLOCK长度以0.5米为单位向下取整；
车头方向为LOCREPORT_DIRECTION_UP。
否则：
车头最小定位所在BLOCK需改为其UP方向的下个BLOCK；
车头最小定位所在坐标为0；
车头方向为LOCREPORT_DIRECTION_DOWN。
ATP shall send the minimum head location of the active cab to the ZC, including the block id and its abscissa. According to [REF5], the unit of the abscissa in Location Report is 0.5 meter, so the ATP needs to convert its internal unit to match that. The conversion shall be safety-oriented, which means the envelope of the train location tend to be "stretched" to the both ends. The rules of conversion are as following ARDL:
```
	def TrainHeadMinLocation(k):
	    if (not TrainLocalized(k)):
	        TrainHeadMinLocation.Block = 0
	        TrainHeadMinLocation.Abscissa= 0
	        TrainHeadOrientation = LOCREPORT_DIRECTION_UNKNOWN
	    elif (TrainFrontLocation(k).Min.Ort is UP):
	        TrainHeadMinLocation.Block = TrainFrontLocation(k).Min.Block(k)
	        TrainHeadMinLocation.Abscissa = (round.floor(TrainFrontLocation(k).Min.Abscissa(k)
	                                                              / ABSCISSA_TO_HALF_METER))
	        TrainHeadOrientation = LOCREPORT_DIRECTION_UP
	    elif (TrainFrontLocation(k).Min.Abscissa(k) + ABSCISSA_TO_HALF_METER
	          <= TrackMap.Blocks[TrainFrontLocation(k).Min.Block].Length):
	        TrainHeadMinLocation.Block = TrainFrontLocation(k).Min.Block(k)
	        TrainHeadMinLocation.Abscissa = round.ceil(TrainFrontLocation(k).Min.Abscissa(k)
	                                                            / ABSCISSA_TO_HALF_METER)
	        TrainHeadOrientation = LOCREPORT_DIRECTION_DOWN
	    else:
	        NextBlock = TrackMap.NextBlock(TrackMap.Blocks[TrainFrontLocation(k).Min.Block], UP)
	        if TrackMap.ExistThePole(TrainFrontLocation(k).Min.Block, NextBlock.Id):
	            TrainHeadMinLocation.Block = NextBlock.Id
	            TrainHeadMinLocation.Abscissa = round.floor(NextBlock.Length
	                                                                / ABSCISSA_TO_HALF_METER)
	            TrainHeadOrientation = LOCREPORT_DIRECTION_UP
	        else:
	            TrainHeadMinLocation.Block = NextBlock.Id
	            TrainHeadMinLocation.Abscissa = 0
	            TrainHeadOrientation = LOCREPORT_DIRECTION_DOWN
	    return TrainHeadMinLocation
```
In above ARDL, the ABSCISSA_TO_HALF_METER means the coefficient of unit conversion.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211], [iTC_CC_ATP_SwHA-0196]
[End]
[iTC_CC_ATP-SwRS-0404]
TrainHeadCoupledStatus，车头连挂状态。
ATP shall send the coupled status of the active train cab to the ZC.
```
	def TrainHeadCoupledStatus(k):
	    if (TrainFrontEnd(k) is END_1):
	        return ((TrainCoupledType(k) is TRAIN_NOT_COUPLED)
	                or (TrainCoupledType(k) is TRAIN_COUPLED_END2))
	    else:
	        return ((TrainCoupledType(k) is TRAIN_NOT_COUPLED)
	                or (TrainCoupledType(k) is TRAIN_COUPLED_END1))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0405]
TrainTailCabId，车尾ID号。
ATP shall send the current inactive cab id to the ZC.
```
	def TrainTailCabId(k):
	    if (TrainFrontEnd(k) is END_1):
	        return END_2
	    else:
	        return END_1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0406]
TrainTailOrientation，ATP需将车尾最小定位的运营方向发送给ZC，作为车位运营方向。规则见SwRS-0408。
ATP shall send the orientation of the minimum location of inactive cab id to the ZC. For the rule can refer to SwRS-0408.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0408]
TrainTailMinLocation，车尾最小定位所在位置。
根据[REF5]，在LocReport中的坐标单位为0.5米，因此需进行单位转换。转换时应当导向ZC处理的安全侧，偏向将列车包络定位“拉长”，即向列车的上游方向取整。
如果列车失位，设置上述定位信息为0；
否则，如果列车车尾朝DOWN方向，则：
车尾最小定位所在BLOCK号不变；
车尾最小定位所在坐标以0.5米为单位向下取整；
车尾方向为LOCREPORT_DIRECTION_DOWN。
否则，如果车尾最小定位坐标加0.5米小于该BLOCK长度，则：
车尾最小定位所在BLOCK号不变；
车尾最小定位所在坐标以0.5米为单位向上取整；
车尾方向为LOCREPORT_DIRECTION_UP。
否则，如果车尾最小定位所在BLOCK找不到UP方向的下个BLOCK，则：
车尾最小定位所在BLOCK号不变，
车尾最小定位所在坐标以0.5米为单位向上取整（此时由于发送的坐标超过了Block长度，ZC会将本LocReport丢弃，不会影响安全）。
车尾方向为LOCREPORT_DIRECTION_UP。
否则，如果车尾最小定位所在BLOCK，与该BLOCK的UP方向下个BLOCK之间存在灯泡线极点，则：
车尾最小定位所在BLOCK需改为其上行方向的下个BLOCK；
车尾最小定位所在坐标为下游BLOCK长度以0.5米为单位向下取整；
车尾方向为LOCREPORT_DIRECTION_DOWN。
否则， 
车尾最小定位所在BLOCK需改为其上行方向的下个BLOCK；
车尾最小定位所在坐标为0；
车尾方向为LOCREPORT_DIRECTION_UP。
ATP shall send the minimum tail location of the active cab to the ZC, including the block id and its abscissa. According to [REF5], the unit of the abscissa in Location Report is 0.5 meter, so the ATP needs to convert its internal unit to match that. The convertion shall be safety-oriented, which means the envelope of the train location tend to be "stretched" to the both ends. The rules of convertion are as following ARDL:
```
	 def TrainTailMinLocation(k):
	    if (not TrainLocalized(k)):
	        TrainTailMinLocation.Block = 0
	        TrainTailMinLocation.Abscissa = 0
	        TrainTailOrientation = LOCREPORT_DIRECTION_UNKOWN
	    elif (TrainRearLocation(k).Min.Ort is DOWN):
	        TrainTailMinLocation.Block = TrainRearLocation(k).Min.Block
	        TrainTailMinLocation.Abscissa = (round.floor(TrainRearLocation(k).Min.Abscissa
	                                                              / ABSCISSA_TO_HALF_METER))
	        TrainTailOrientation = LOCREPORT_DIRECTION_DOWN
	    elif (TrainRearLocation(k).Min.Abscissa + ABSCISSA_TO_HALF_METER
	          <= TrackMap.Block[TrainRearLocation(k).Min.Block].Length):
	        TrainTailMinLocation.Block = TrainRearLocation(k).Min.Block
	        TrainTailMinLocation.Abscissa = round.ceil(TrainRearLocation(k).Min.Abscissa
	                                                            / ABSCISSA_TO_HALF_METER)
	        TrainTailOrientation = LOCREPORT_DIRECTION_UP
	    else:
	        NextBlock = TrackMap.NextBlock(TrackMap.Blocks[TrainRearLocation(k).Min.Block], UP)
	        if (NextBlock is None):
	            TrainTailMinLocation.Block = TrainFrontLocation(k).Min.Block
	            TrainTailMinLocation.Abscissa = round.ceil(TrainRearLocation(k).Min.Abscissa
	                                                                / ABSCISSA_TO_HALF_METER)
	            TrainTailOrientation = LOCREPORT_DIRECTION_UP
	        elif (TrackMap.ExistThePole(TrainRearLocation(k).Min.Block, NextBlock.Id)):
	            TrainTailMinLocation.Block = NextBlock.Id
	            TrainTailMinLocation.Abscissa = round.floor(NextBlock.Length
	                                                                / ABSCISSA_TO_HALF_METER)
	            TrainTailOrientation = LOCREPORT_DIRECTION_DOWN
	        else:
	            TrainTailMinLocation.Block = NextBlock.Id
	            TrainTailMinLocation.Abscissa = 0
	            TrainTailOrientation = LOCREPORT_DIRECTION_UP
	    return TrainTailMinLocation
```
In above ARDL, the ABSCISSA_TO_HALF_METER means the coefficient of unit convertion.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211], [iTC_CC_ATP_SwHA-0196]
[End]
[iTC_CC_ATP-SwRS-0409]
TrainTailCoupledStatus，车尾连挂状态。
ATP shall send the coupled status of the inactive train cab to the ZC.
```
	def TrainTailCoupledStatus(k):
	    if (TrainFrontEnd(k) is END_1):
	        return ((TrainCoupledType(k) is TRAIN_NOT_COUPLED)
	                or (TrainCoupledType(k) is TRAIN_COUPLED_END1))
	    else:
	        return ((TrainCoupledType(k) is TRAIN_NOT_COUPLED)
	                or (TrainCoupledType(k) is TRAIN_COUPLED_END2))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0410]
LocationError，最大最小定位误差. 根据[REF5]，在LocReport中的长度单位是0.5米，因此需进行转换，转换时应当导向安全侧。
ATP shall send the location error to the ZC. According to [REF5], the unit of the location error in Location Report is 0.5 meter, so the ATP needs to convert its internal unit to match that. The convertion shall be safety-oriented, which means the location error tend to be "overestimated".
```
	def LocationError(k):
	    if TrainLocalized(k):
	        return round.ceil((TrainLocation(k).Uncertainty + ABSCISSA_TO_HALF_METER)
	                              / ABSCISSA_TO_HALF_METER)
	    else:
	        return 0
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0212], [iTC_CC_ATP_SwHA-0196]
[End]
[iTC_CC_ATP-SwRS-0068]
RouteSetNotNeededSendable，是否可以发送RSNN信息。其状态来自于项目可配置的列车输入采集。
According to the status of RouteSetNotNeededSendable, ATP can judge whether it is necessary to send RSNN information.
```
	def RouteSetNotNeededSendable(k):
	    return Offline.GetRouteSetNotNeededSendable()
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0215], [iTC_CC-SyAD-0237], [iTC_CC-SyAD-0341], [iTC_CC-SyAD-1003], [iTC_CC-SyAD-1311], [iTC_CC_ATP_SwHA-0204]
[End]
[iTC_CC_ATP-SwRS-0135]
NonVitalRouteSetNotNeeded，RSNN状态
Whether the CCNV request route set note needed.
```
	def NonVitalRouteSetNotNeeded(k):
	    return (ATOcontrolTimeValid(k)
	            and NonVitalRequest.RouteSetNotNeeded(k))
```
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0215], [iTC_CC-SyAD-0408], [iTC_CC-SyAD-1044]
[End]
[iTC_CC_ATP-SwRS-0414]
TrainRouteSetNotNeeded，是否发送RSNN信息。
ATP shall send the route set not needed information to ZC.
```
	def TrainRouteSetNotNeeded(k):
	    return (TrainFilteredStopped(k)
	            and NonVitalRouteSetNotNeeded(k)
	            and RouteSetNotNeededSendable(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0215], [iTC_CC-SyAD-0237], [iTC_CC_ATP_SwHA-0210]
[End]
[iTC_CC_ATP-SwRS-0415]
TrainCorrectDocking，列车是否正确停靠车站。
ATP shall send the docking correction information to the ZC.
```
	def TrainCorrectDocking(k):
	    return (EnableDoorOpening_A(k) or EnableDoorOpening_B(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0216]
[End]
[iTC_CC_ATP-SwRS-0416]
LocReportSpeed，列车最大速度，需转换为CC-ZC接口协议中的单位，并向上取整。
ATP shall send the maximum train speed to the ZC. According to [REF5], the unit of the speed in Location Report is KPH, so the ATP needs to convert its internal unit to match that. The convertion shall be safety-oriented, which means the speed tend to be "overestimated".
```
	def LocReportSpeed(k):
	    return round.ceil(TrainMaxSpeed(k) / KMPH_TO_MMPS)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0217], [iTC_CC_ATP_SwHA-0272]
[End]
[iTC_CC_ATP-SwRS-0417]
TrainMonitoringMode，监控模式.
ATP shall send the current monitoring mode to the ZC.
```
	def TrainMonitoringMode(k):
	    if (MotionProtectionInhibition(k)
	         and RMRselectedDrivingMode(k)):
	        return RMR
	    elif (MotionProtectionInhibition(k)):
	        return RMF
	    else:
	        return OTHERS
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0218], [iTC_CC-SyAD-0344], [iTC_CC_ATP_SwHA-0212]
[End]
[iTC_CC_ATP-SwRS-0599]
SignalOverrideSendable，发给ZC的关信号机命令。
```
	def SignalOverrideSendable(k):
	    return Offline.GetSignalOverrideSendable(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1312]
[End]
[iTC_CC_ATP-SwRS-0418]
SignalsOverride，是否CBTC下取消信号。
ATP shall send the signal override information to the ZC.
```
	def SignalsOverride(k):
	    return (SignalOverrideSendable(k)
	            and not MotionProtectionInhibition(k)
	            and (ATOcontrolTimeValid(k)
	                 and NonVitalRequest.CancelSignal))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0207], [iTC_CC-SyAD-0219], [iTC_CC_ATP_SwHA-0221]
[End]
[iTC_CC_ATP-SwRS-0598]
ATCcontrolledTrain，ATP未被切除。
```
	def ATCcontrolledTrain(k):
	    return Offline.GetATCcontrolledTrain(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1216], [iTC_CC-SyAD-1296], [iTC_CC-SyAD-1297], [iTC_CC-SyAD-1306]
[End]
