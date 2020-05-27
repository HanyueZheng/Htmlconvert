
文档[REF10]描述了列车能量的监控原理与实现方法，在EOA有效前提下，ATP根据过估的线路坡度和运动学属性，计算列车以当前速度和最大加速度条件下牵引切除紧急制动施加瞬间的速度和位置，作为列车的最大能量点。并以该点的能量，依次与车身范围及下游限制点的能量进行比较，判断列车是否超能。即如果列车超能，则ATP应当输出紧急制动，确保列车在最恶劣条件下不会超出线路限制速度以及冒进下游的EOA。
Document [REF10] describes the principle and implementation of the train energy monitoring. When the EOA is valid, ATP calculates the maximum energy position, where the traction has cut off and the brake begin to effect, based on the overestimated gradient and train kinematics. According to the principle of conservation of energy, ATP uses the train maximum energy to compare with the restricted energy of the vital zones train located or the downstream constraint points, to determine whether the current train energy exceeded the environment limits. If the over energy was detected, ATP shall request emergency braking to ensure that in the worst conditions, the train will not exceed the vital zone's speed limits or will not overrun the downstream EOA.
[iTC_CC_ATP-SwRS-0309]
X1TractionCutoff，V1TractionCutoff，在EndOfAuthorityValid为True前提下，ATP根据列车当前最大速度TrainMaxSpeed，当前速度下的最大牵引力加速度（使用列车最小速度TrainMinSpeed在配置数据中查找），车头最大定位所在Block的坡度最大加速度（来自配置数据），计算出经过时间后列车行驶的距离和所达到的速度。
If EOA is valid, ATP shall calculate the distance and the speed of the train moved after traction cutoff period (), according to the current train maximum speed, the maximum acceleration of traction and the maximum acceleration of the gradient.
```
	
	
```
其中，
，从ATP发出EB指令到列车牵引切除的时间ATPsetting.EBtractionCutoffLatency
来自配置数据，是当前速度为时，列车牵引力所能提供的最大加速度ATPsetting. TractionMaxAcc；
来自线路地图，是离线工具从当前所在block起始点上游ATPsetting. LocationMaxUncertaintyConfirmed开始，记录的长度为（）的轨道区域内的坡度变化点中最大的坡度加速度。
Where,
，the latency from ATP trigger EB command to the traction cut off.
, from ATPsetting, is the maximum acceleration of traction at current speed. 
, from TrackMap, is the maximum acceleration of the gradient in the block start point train front end located plus to the distance ()
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0306], [iTC_CC-SyAD-0307], [iTC_CC-SyAD-0315], [iTC_CC_ATP_SwHA-0127], [iTC_CC-SyAD-1004], [iTC_CC-SyAD-0308]
[End]
[iTC_CC_ATP-SwRS-0310]
X2EbApplied，V2EbApplied，在EndOfAuthorityValid为True前提下，根据车头最大定位所在Block的坡度最大加速度，计算出经过牵引切除(t1)和EB施加(t2)两部分时间后列车行驶的距离X2EbApplied和达到的速度V2EbApplied。
If EOA is valid, ATP shall calculate the distance and the speed of the train moved after the traction cutoff period plus emergency brake applied period (), according to the current train maximum speed, the maximum acceleration of traction and the maximum acceleration of the gradient.
```
	
```
Where,
```
	
```
，the ATPsetting.EBtractionToBrakingLatency latency from RS cut off the traction to the emergency braking applied.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0306], [iTC_CC-SyAD-0315], [iTC_CC_ATP_SwHA-0127], [iTC_CC-SyAD-1004], [iTC_CC-SyAD-0308]
[End]
[iTC_CC_ATP-SwRS-0312]
TrainEnergy，计算EB施加时刻的列车动能，作为能量监控使用的列车能量。
ATP shall calculate the train energy where EB indeed applied. The calculation shall consider the kinetic energy and the error of the potential energy. 
```
	TrainEnergy = V2EbApplied * V2
	EbApplied               + ATPsetting.MPauthAltitudeMaxErrorEnergy
```
The ATPsetting.MPauthAltitudeMaxErrorEnergy means an algorithm error caused by offline tool to calculate the compensation gradients.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0305], [iTC_CC-SyAD-0306], [iTC_CC-SyAD-0312], [iTC_CC-SyAD-0313], [iTC_CC_ATP_SwHA-0127]
[End]
