
[iTC_CC_ATP-SwRS-0270]
RestrictiveSignalOverrun，BM模式下，本周期列车车头最大定位是否冒进限制状态的信号机。
当满足以下所有条件时，ATP认为列车冒进了限制状态的信号机，需设置RestrictiveSignalOverrun为True。
本周期列车已定位，即TrainLocalized为True；
本周期使用BM变量；
上周期RestrictiveSignalOverrun为False；
本周期列车位移MaximumTrainMotion向激活的驾驶室方向运行；
本周期列车车头最大定位TrainFrontLocation经过了一个信号机奇点；
该信号机为限制状态，或者建立了Overlap的状态。
否则，设置RestrictiveSignalOverrun为False。
RestrictiveSignalOverrun, ATP shall determine whether the location of maximum train head overruns a restricted signal in BLOCK mode.
When all of the following conditions fulfilled, ATP considers the train has overrun a restricted signal in this cycle, and set RestrictiveSignalOverrun as True.
Train has localized;
And the current type of EOA is BLOCK_MODE_EOA;
And RestrictiveSignalOverrun was False at the last cycle;
And the moving direction in current cycle is toward on the train front end;
And the maximum location of train front end passes the position of the signal in this cycle;
And the status of the signal is restriction or overlap established.
Otherwise, ATP set RestrictiveSignalOverrun as False.
```
	def RestrictiveSignalOverrun(k):
	    sing = TrackMap.ExistSingBtwTwoLocs(SGL_SIGNAL, TrainFrontLocation(k-1).Max,
	                                               TrainFrontLocation(k).Max)
	    return (sing is not None
	            and BMvariantValidWhileTemporallyValid(k)
	            and ((TrainFrontEnd(k) is END_2 and End2RunningForward(k))
	                 or (TrainFrontEnd(k) is END_1 and End1RunningForward(k)))
	            and not BMvariantValue(sing.Variant, k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0290], [iTC_CC_ATP_SwHA-0182]
[End]

Figure 517 Train located in BM initialZone
BM模式的列车只有在BM初始化区域内才能获得移动授权，主要为防止列车下游到计轴边界范围内有未被ATP检测到的其他车辆。该区域定义为带BM初始化属性的信号机下游第一个Block边界，至边界上游ATPsetting.BMinitAreaLength长度的区域，如Figure5-15所示。
注意，项目设计时，带有BM初始化属性的信号机奇点不能设置在运营方向的Block起始点处，而应设置在其上游Block的末端（实际上这两点是同一位置），就是说：
对于UP方向，BM初始化信号机不应设置在某Block的坐标0点处；
对于Down方向，BM初始化信号机不应设置在某Block的最大坐标（即坐标为Block长度）处。
[iTC_CC_ATP-SwRS-0662]
TrainInBMinitialZone，车头最小定位在在BM初始化区域内。
```
	def TrainInBMinitialZone(k):
	    NewBlock = TrackMap.ExistSingularityInZone(SGL_NEW_BLOCK, TrainFrontLocation(k).Min,
	                                                           ATPsetting.BMinitAreaLength)
	    Signal = (TrackMap.ExistSingularityInReverseZone(SGL_SIGNAL,
	                                                              NewBlock.Location,
	                                                              ATPsetting.BMinitAreaLength))
	    if (Signal is not None
	        and Signal.BmInitialization):
	        return Signal
	    else:
	        return None
```
其中NewBlock.Location表示block的起始位置。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1284], [iTC_CC-SyAD-0291]
[End]
[iTC_CC_ATP-SwRS-0663]
TrainEnteredInBMinitialZoneAge， 如果列车在BM初始化区域内，则记录已在该区域内运行的时间
```
	def TrainEnteredInBMinitialZoneAge(k):
	    if (TrainInBMinitialZone(k) is None):
	        TrainEnteredInBMinitialZoneAge = 0
	    else:
	        TrainEnteredInBMinitialZoneAge = TrainEnteredInBMinitialZoneAge(k-1) + 1
	    return TrainEnteredInBMinitialZoneAge 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1284]
[End]
[iTC_CC_ATP-SwRS-0664]
StopAssuredPointCrossed，本周期是否通过了信号机前方的BMCP点
```
	def StopAssuredPointCrossed(Cbi, k):
	    Bmcp = TrackMap.ExistSingBtwTwoLocs(SGL_BMCP, TrainFrontLocation(k-1).Max,
	                                               TrainFrontLocation(k).Max)
	    return (Bmcp is not None
	             and cbi == Bmcp.CbiId)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1177]
[End]
[iTC_CC_ATP-SwRS-0665]
CBIminProductionAgeSinceSSAcrossing，记录从通过上个BMCP点开始到现在已经过了多长时间
```
	def CBIminProductionAgeSinceSSAcrossing(Cbi, k):
	    if (Initialization
	        or CBIminProductionAgeSinceSSAcrossing(k-1)>= REPORT_AGE_MAX):
	        return REPORT_AGE_MAX
	    elif (StopAssuredPointCrossed(Cbi, k)):
	        return ATPsetting.VariantsBMALSpresenceTimer
	    else:
	        return (CBIminProductionAgeSinceSSAcrossing(Cbi, k-1) + 1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1177]
[End]
[iTC_CC_ATP-SwRS-0666]
CBIminProductionAge，对于每个联锁，ATP维护最后收到其变量消息时联锁的最小时间，到现在经过的时间。
```
	def CBIminProductionAge(cbi, k):
	    return min(CBIminProductionAgeSinceSSAcrossing(Cbi, k),
	                 CBIvariantReportLastAge(Cbi, k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1175]
[End]
[iTC_CC_ATP-SwRS-0667]
ReceivedVariantsAfterEnteredBMinitialZone，先进入BM初始化区，再收到无线或者信标的变量
```
	def ReceivedVariantsAfterEnteredBMinitialZone(k):
	    CbiId = TrackMap.CbiId(TrainInBMinitialZone(k).Block)
	    return (TrainInBMinitialZone(k) is not None
	            and ((CBIvariantMoreAvailableThanBeacon(CbiId, k)
	                  and ((CBIvariantReportLastAge(CbiId, k)
	                         + ATPsetting.VariantsBMproductionLatencyRadio)
	                       < TrainEnteredInBMinitialZoneAge(k)))
	                 or (BMbeaconReadAge(k) + ATPsetting.VariantsBMproductionLatencyBeacon
	                     < TrainEnteredInBMinitialZoneAge(k))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1284]
[End]
[iTC_CC_ATP-SwRS-0504]
BlockModeEOAvalid，BM下的移动授权是否可用
```
	def BlockModeEOAvalid(k):
	    if (Initialization
	        or not BlockModeUsed(k)
	        or TrainFrontEnd(k) is not TrainFrontEnd(k-1)
	        or not TrainLocatedOnKnownPath(k)
	        or HazardousMotionOnNonExclusiveRoute(k)
	        or RestrictiveSignalOverrun(k)):
	        return False
	    elif (not BlockModeEOAvalid(k-1)
	          and TrainInBMinitialZone(k) is not None
	          and BMvariantValue(TrainInBMinitialZone.Variant(k), k)
	          and ReceivedVariantsAfterEnteredBMinitialZone(k)):
	        return True
	    else:
	        return BlockModeEOAvalid(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0290], [iTC_CC-SyAD-0293], [iTC_CC-SyAD-1037], [iTC_CC-SyAD-0291], [iTC_CC-SyAD-0292], [iTC_CC_ATP_SwHA-0047], [iTC_CC_ATP_SwHA-0048], [iTC_CC_ATP_SwHA-0050]
[End]
