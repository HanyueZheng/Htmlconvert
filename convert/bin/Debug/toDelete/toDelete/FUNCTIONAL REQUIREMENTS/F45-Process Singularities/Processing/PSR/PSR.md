
PSR作为限制区域：列车车尾最小定位到EB实际施加位置的范围内的线路永久限速，需作为安全速度限制区域，其限速值为该PSR奇点中的属性值。
PSR作为限制点：限速减小的PSR起始点，其限速值为线路地图中该PSR奇点的属性值。
限制区能量
限制点能量

Figure 518 PSR as vital speed limit zone
[iTC_CC_ATP-SwRS-0691]
ZoneVSLnotExceedPSR，PSR作为区域型限速的情形，ATP应将以下两种类型的PSR作F为限制区域进行监控：
该PSR是车尾最小定位上游的第一个PSR（即从该PSR所在位置到车尾最小定位之间没有其他PSR），如Figure 518中的PSR2；
该PSR位于车尾最小定位下游到EB实际位置之间，如Figure 518中的PSR2,PSR3和PSR4。
```
	def ZoneVSLnotExceedPSR(k):
	    for Psr in TrackMap.AllSingsBtwTwoLocs(SGL_PSR,
	                                                   TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                                                   TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                                      X2EbApplied(k))):
	        if (TrainEnergy(k) >= pow(Psr.SpeedLimit)
	             and (not TrackMap.LocationBtwTwoLocs(Psr.Location,
	                                                        TrackMap.BlockOrigin(TrainRearLocation(k).Min),
	                                                        TrainRearLocation(k).Min)
	                   or (TrackMap.ExistSingBtwTwoLocs(SGL_PSR, Psr.Location,
	                                                          TrainRearLocation(k).Min) is None))):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0284], [iTC_CC-SyAD-0285], [iTC_CC-SyAD-0300], [iTC_CC_ATP_SwHA-0261]
[End]
[iTC_CC_ATP-SwRS-0692]
PointVSLnotExceedPSR，PSR作为点型限速的情形
```
	def PointVSLnotExceedPSR(k):
	    for Psr in (TrackMap.AllSingsInZone(SGL_PSR,
	                                TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                                ATPsetting.EOAmaxDistance)):
	        if (TrainEnergy(k) >= (pow(Psr.SpeedLimit)
	                                   + (Energy.AccumulationPotentialEnergy
	                                       (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                           X2EbApplied(k)),
	                                        Psr.Location)))):
	            return False 
	        else:
	            continue
	    return True
```
其中，Energy.AccumulationPotentialEnergy表示根据限制点所在坡度或EB最小保障率累加计算目标位置的势能，EB最小保障率应根据所在位置的Grip值（Normal或Reduce）选取ATPsetting.EBguaranteedAccNormalGrip或ATPsetting.EBguaranteedAccReducedGrip。能量计算的原理和方法见[REF10]。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0284], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC-SyAD-0285], [iTC_CC_ATP_SwHA-0261]
[End]
NOTES:
当车尾在一个较低的PSR（或TSR）中时，若当前车速小于该PSR限速，而计算出的V2速度大于该PSR限速，按照上述处理方式，也会导致EB，尽管当列车运行到X2位置时，列车也许已经离开了该PSR区域。
If the train tail intersected with a PSR (or TSR) area, and the speed of train is lower but the V2EbApplied is higher than the limitation. In accordance with the above approach will result in EB, although when the train runs to the EB applied position, the train may have left the PSR area.
