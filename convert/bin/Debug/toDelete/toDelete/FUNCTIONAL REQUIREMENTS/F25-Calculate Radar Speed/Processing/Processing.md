
对于配置有多普勒雷达的项目，ATP软件每周期需获取雷达的测速信息，按照配置参数过估后，作为计算列车速度的依据。
[iTC_CC_ATP-SwRS-0781]
RadarInfo，ATP软件根据来自VPB板的雷达寄存器，计算雷达信息。

|Identification|Logical Type|Description|
|-----|
|ST_RADAR_INFO|||
||CbkMovement| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|根据雷达脉冲信息计算的周期位移|
||CbkSpeed| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|根据雷达脉冲信息计算的周期速度|
||DrsDistance| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|根据雷达双通道位移信息累计的运行距离|
||DrsMovement| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|根据雷达双通道位移信息计算的周期位移|
||DrsSpeed| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|根据雷达速度信息计算的周期速度|
||DrsValid| REF BOOLEAN BOOLEAN|雷达信息有效（TBD，应根据上述信息相互校验）|
||DrsDirection| REF NUMERIC_32 \h NUMERIC_32|向前为1，向后为0|

\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=
[End]
[iTC_CC_ATP-SwRS-0782]
RadarRawSpeed，直接获取的雷达测速值，该值始终为正。
```
	def RadarRawSpeed(k):
	    if RadarInfo(k).DrsValid:
	        return RadarInfo(k).DrsSpeed
	    else:
	        return MAX_RADAR_SPEED
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=
[End]
[iTC_CC_ATP-SwRS-0783]
RadarDirection，雷达测得方向，向End1方向为+1，向End2为-1，其余为0
```
	def RadarDirection(k):
	    if not RadarInfo(k).DrsValid:
	        return 0
	    elif ((CoreId(k) is END_1
	           and RadarInfo(k).DrsDirection > 0)
	         or (CoreId(k) is END_2
	             and RadarInfo(k).DrsDirection <= 0)):
	        return 1
	    else:
	        return -1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=
[End]
ATP应根据项目配置信息对雷达测得的速度进行过估，方式如下：
当原始测得速度大于配置的阈值ATPsetting.RadarSpeedThreshold时，最大最小位移将分别按照百分比ATPsetting.RadarDeviationAboveThreshold过估；
当原始测得速度小于配置的阈值ATPsetting.RadarSpeedThreshold时，最大最小位移将按照固定速度值ATPsetting.RadarDeviationBelowThreshold过估，速度最小为0。
[iTC_CC_ATP-SwRS-0784]
RadarMotionMax，绝对值向上过估的雷达最大位移，向END1方向该值为正，向END2方向该值为负。
```
	def RadarMotionMax(k):
	    if not RadarInfo(k).DrsValid:
	        return 0
	    elif RadarRawSpeed(k) >= ATPsetting.RadarSpeedThreshold:
	        return (ATP_CYCLE_TIME * RadarDirection(k) * (RadarRawSpeed(k)
	                + RadarRawSpeed(k) * ATPsetting.RadarDeviationAboveThreshold / 1000))
	    else:
	        return (ATP_CYCLE_TIME * RadarDirection(k)
	                 * (RadarRawSpeed(k) + ATPsetting.RadarDeviationBelowThreshold))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=
[End]
[iTC_CC_ATP-SwRS-0785]
RadarMotionMin，绝对值向下过估的雷达最小位移，向END1方向该值为正，向END2方向该值为负。
```
	def RadarMotionMin(k):
	    if not RadarInfo(k).DrsValid:
	        return 0
	    elif RadarRawSpeed(k) >= ATPsetting.RadarSpeedThreshold:
	        return (ATP_CYCLE_TIME * RadarDirection(k) * (RadarRawSpeed(k)
	                - RadarRawSpeed(k) * ATPsetting.RadarDeviationAboveThreshold / 1000))
	    else:
	        return (ATP_CYCLE_TIME * RadarDirection(k)
	                 * max(0, RadarRawSpeed(k) - ATPsetting.RadarDeviationBelowThreshold))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=
[End]
NOTES：
仅当项目配置了雷达，且雷达最大最小位移范围与里程计经打滑空转补偿后的最大最小位移范围有交集时，才认为雷达速度有效。现场数据表明，列车持续减速，当里程计读值变为0后，雷达会延迟超过1秒，测速结果才会变为0，此时不能认为雷达测速无效。因此，只有当雷达测得方向与里程计经打滑补偿后的测得方向相反时，才认为雷达无效。
[iTC_CC_ATP-SwRS-0786]
RadarSpeedValid，判断雷达速度是否可用
```
	def RadarSpeedValid(k):
	    return (ATPsetting.RadarApplied
	            and  RadarInfo(k).DrsValid
	            and (not ValidSlipSlideModelling(k)
	                 or (RadarDirection(k) * MaximumSScompensatedMotion(k) >= 0
	                     and abs(RadarMotionMin(k)) <= abs(MaximumSScompensatedMotion(k))
	                     and abs(RadarMotionMax(k)) >= abs(MinimumSScompensatedMotion(k)))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=
[End]
