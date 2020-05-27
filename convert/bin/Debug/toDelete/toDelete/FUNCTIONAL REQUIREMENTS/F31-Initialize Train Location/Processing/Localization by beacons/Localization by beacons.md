
[iTC_CC_ATP-SwRS-0461]
End2OrientationByBeacon，当定位初始化时，通过经过的信标，判断END_2驾驶室所面对的运营方向。
The orientation of the train END means the UP or DOWN orientation which this END toward to. When a pair of consecutive beacon read, ATP can determine the orientation for each train END according to the direction of these beacons in track map and the direction of train movement.
```
	def End2OrientationByBeacon(k):
	    if (Initialization
	        or not MovingInitialByBeacon(k)):
	        return DOT_UNKNOWN
	    elif (ATPsetting.PolarizedTrain):
	        return ATPsetting.End2Orientation
	    elif (NewBeaconObtained(k)
	          and TrackMap.AreNeighbouredBeacons(BeaconBeforeLastObtained(k),
	                                                    BeaconLastObtained(k))):
	        if (End2RunningForward(k)):
	            return (TrackMap.OrientationOfNeighbouredBeacons(BeaconBeforeLastObtained(k),
	                                                                       BeaconLastObtained(k)))
	        else:
	            return (TrackMap.OrientationOfNeighbouredBeacons(BeaconLastObtained(k),
	                                                                       BeaconBeforeLastObtained(k)))
	    else:
	        return End2OrientationByBeacon(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0177], [iTC_CC-SyAD-0180], [iTC_CC-SyAD-0211], [iTC_CC-SyAD-1197], [iTC_CC-SyAD-1198], [iTC_CC-SyAD-0183], [iTC_CC_ATP_SwHA-0102], [iTC_CC_ATP_SwHA-0240]
[End]
Figure 512 Determine orientation by two neighbor beacons
NOTES：
对于极性车，每个车头的运营方向是固定的，如END_1端车头只能向“上行”运营，作为项目配置存储在ATP数据中。
对于对于非极性车，当读到信标时，ATP会根据列车运行方向（即向END_2方向运行或者END_1方向运行）和连续两个信标的“指向”来判断运行方向：
对于两信标间没有灯泡线“极点”的情况
如果列车向END_2方向运行，而先后读到的信标从A到B为“上行”方向，则END_2端为“上行”，如Figure 512所示；
如果列车向END_2方向运行，而先后读到的信标从A到B为“下行”方向，则END_2端为“下行”；
如果列车向END_1方向运行，而先后读到的信标从A到B为“上行”方向，则END_2端为“下行”；
如果列车向END_1方向运行，而先后读到的信标从A到B为“下行”方向，则END_2端为“上行”；
如果相邻两个信标之间有灯泡线的“极点”，则该对信标不能用于方向判别，即无法通过该对信标进行列车定位初始化。
NOTES：
所谓灯泡线，是指列车无需折返更换驾驶室，即可完成运营方向由上行切换到下行（或反之）的作业线路。如Figure 513所示，若列车车身跨UP极点运行时，则END_1和END_2的方向均为DOWN。
On a balloon loop, a train can run to the reverse direction without having to shunt or even stop, refer to Figure 513,which means there are inconsistent orientations for two train ENDs (both END_1 and END_2 are toward to UP or DOWN).
Figure 513 The balloon loop
[iTC_CC_ATP-SwRS-0248]
BeaconLocation，如果本周期读到了重定位信标（无论是否已经在定位状态），则ATP需根据该信标在线路地图中的坐标计算读到信标时刻END_2车头的位置：
If a valid beacon read, ATP shall calculate actual maximum and minimum location of the END_2 according to the beacon location in track map, the distance from beacon antenna to the END_2, the distance after top-loc, the orientation of END_2 and the direction of train movement. The beacon location indicate the actual external location of the END_2.
```
	def BeaconLocation(k):
	    if (NewBeaconObtained(k)):
	        if (End2OrientationByBeacon(k) is not None):
	            BeaconLocation.Ext2 = (TrackMap.LocationUpdateExt2
	                                       (End2RunningForward(k),
	                                        End2OrientationByBeacon(k),
	                                        BeaconLastObtained.Location(k),
	                                        MaxMotionOfEnd2(k),
	                                        MinMotionOfEnd2(k)))
	        elif (TrainLocalized(k-1)):
	            BeaconLocation.Ext2 = (TrackMap.LocationUpdateExt2
	                                       (End2RunningForward(k),
	                                        TrainLocation(k-1).Ext2.Ort,
	                                        BeaconLastObtained.Location(k),
	                                        MaxMotionOfEnd2(k),
	                                        MinMotionOfEnd2(k)))
	        else:
	            BeaconLocation.Ext2 = None
	            
	        BeaconLocation.Uncertainty = (2 * BeaconLastObtained(k).PositionTolerance
	                                            + abs(DistLastBeaconMax(k) - DistLastBeaconMin(k))) 
	        BeaconLocation.Int2 = U
	pdateInt2FromExt2        BeaconLocation.Ext1 = U
	pdateExt1FromExt2        BeaconLocation.Int1 = U
	pdateInt1FromExt2    else:
	        BeaconLocation = None
	    return BeaconLocation
```
其中，所用到的内部函数分别为：
```
	def MaxMotionOfEnd2(k)
	    return (DistLastBeaconMax(k)
	            - ATPsetting.CCcoreEnd2BeaconAntennaDistance[CoreId(k)]
	            - BeaconLastObtained(k).PositionTolerance)
	def MinMotionOfEnd2(k)
	    return (DistLastBeaconMin(k)
	            - ATPsetting.CCcoreEnd2BeaconAntennaDistance[CoreId(k)]
	            - BeaconLastObtained(k).PositionTolerance)
```
UpdateInt2FromExt2，根据Figure 514，通过Ext2定位计算End2端的内侧定位Int2。如果计算范围内有非受控道岔或者轨道尽头，则Int2定位应设置为None。如果Int2与Ext2之间有灯泡线极点，则二者的方向将不同。
UpdateExt1FromExt2，根据Figure 514，通过Ext2定位计算End1端的外侧定位Ext1。如果计算范围内有非受控道岔或者轨道尽头，则Ext1定位应设置为None。如果Ext1与Ext2之间有灯泡线极点，则二者的方向将相同。
UpdateInt1FromExt2，根据Figure 514，通过Ext2定位计算End1端的内侧定位Int1。如果计算范围内有非受控道岔或者轨道尽头，则Int1定位应设置为None。如果Int1与Ext2之间有灯泡线极点，则二者的方向将相同。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0176], [iTC_CC-SyAD-0178], [iTC_CC-SyAD-0180], [iTC_CC-SyAD-0182], [iTC_CC-SyAD-0196], [iTC_CC-SyAD-0197], [iTC_CC-SyAD-0202], [iTC_CC_ATP_SwHA-0098], [iTC_CC-SyAD-0183]
[End]
NOTES:
如Figure 514所示，表示列车两端车头的内外侧四个定位的关系。
其中对于每一端车头的内外侧定位，其坐标相差一个定位误差长度。
而对于一端的外侧定位和另一端的内侧定位，其坐标相差配置数据中的列车长度。
对于每一个定位，均有其相应的运营方向。注意，如果车身范围内有灯泡线极点，则两端车头的运营方向可能相同。
Figure 514 Train location with orientation
[iTC_CC_ATP-SwRS-0276]
MovingInitialByBeacon，是否在信标初始化定位过程中。
```
	def MovingInitialByBeacon(k):
	    if (Initialization
	        or TrainLocalized(k-1)
	        or TrainFilteredStopped(k)
	        or (End1RunningForward(k) and not End1RunningForward(k-1))
	        or (End2RunningForward(k) and not End2RunningForward(k-1))
	        or abs(DistLastBeaconMax(k)) >= ATPsetting.BeaconPairMaxDistance):
	        return False
	    elif (NewBeaconObtained(k)):
	        return True
	    else:
	        return MovingInitialByBeacon(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source= [iTC_CC-SyAD-0176], [iTC_CC-SyAD-0180], [iTC_CC-SyAD-0183]
[End]
[iTC_CC_ATP-SwRS-0280]
TrainLocatedOnBeacon，列车通过信标进行初始化定位（该值仅在处理信标的周期为True）。如果在定位初始化阶段读到信标，且能够根据该信标的位置计算出列车的定位，（即车身范围内没有轨道边界或未知状态的道岔），即认为初始化定位成功。
```
	def TrainLocatedOnBeacon(k):
	    return (MovingInitialByBeacon(k)
	              and NewBeaconObtained(k)
	              and (ATPsetting.PolarizedTrain
	                    or (MovingInitialByBeacon(k-1)
	                         and TrackMap.AreNeighbouredBeacons(BeaconBeforeLastObtained(k),
	                                                                   BeaconLastObtained(k))))
	              and BeaconLocation(k) is not None)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source= [iTC_CC-SyAD-0176], [iTC_CC-SyAD-0177], [iTC_CC-SyAD-0180], [iTC_CC-SyAD-0183], [iTC_CC-SyAD-1197], [iTC_CC-SyAD-1198], [iTC_CC_ATP_SwHA-0101]
[End]
