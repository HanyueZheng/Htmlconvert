
在BLOCK运营模式下，线路上的变量信息来自有源信标消息的解析。对于每个有源信标，有其特定的方向，且最多存储16个变量的状态。对于每个变量所代表的线路设备的含义及其有效期，在离线数据中进行配置。在BLOCK运营模式下，ATP仅存储与列车运营方向相同最新的一个BM信标的变量。
In the block mode, the variants come from the parsing of BM beacon message. For each BM beacon, has a specific orientation and can store 16 variants at most. For the content and validity of each variant is defined in the off-line data. During the block mode, ATP shall only store the last read BM variants with the same direction as the train moving.
[iTC_CC_ATP-SwRS-0146]
BeaconVariantsUpdating，判断是否要更新BM变量。
若本周期满足以下所有条件时，则认为需要更新BM变量，设置BeaconVariantsUpdating为True。
当前使用BM变量（BMvariantValidWhileTemporallyValid）；
本周期未停车且收到信标消息且判断该信标带有BM变量；
上周期列车未定位，或该BM信标方向与列车运营方向一致。
否则，设置BeaconVariantsUpdating为False。
BeaconVariantsUpdating used to determine ATP whether to update the BM variants in this cycle.
If all the following conditions are fulfilled, ATP shall set BeaconVariantsUpdating as True:
The current operational mode is BLOCK MODE;
And train moved and ATP received a BM beacon in this cycle;
And the train is either not localized, or the direction of the BM variants is as same as the orientation of the train front end.
Otherwise, ATP shall set BeaconVariantsUpdating as False.
```
	def BeaconVariantsUpdating(k):
	    return (BMvariantValidWhileTemporallyValid(k)
	            and BeaconMessageReceive(k)
	            and TrackMap.IsBmBeacon(BeaconMessage.ID)
	            and not TrainFilteredStopped(k)
	            and (not TrainLocalized(k-1)
	                 or (TrackMap.BmBeaconDirection(BeaconMessage.ID)== TrainFrontOrientation(k-1))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0166], [iTC_CC-SyAD-0298], [iTC_CC-SyAD-0299], [iTC_CC_ATP_SwHA-0040]
[End]
用于比较来自信标的BM变量与来自无线的BM变量哪个更新。
[iTC_CC_ATP-SwRS-0617]
BMbeaconReadAge，记录读取BM信标到当前的时间，默认值为REPORT_AGE_MAX。
如果BM信标变量无效，该值应被设置为默认值，BM信标变量无效的条件如下：
初始化；
或当前不在BM模式(not BMvariantValidWhileTemporallyValid)；
或BMbeaconReadAge已大于ATPsetting.VariantsBMfullValidityTime；
或本周期收到的BM信标（BeaconVariantsUpdating为True）中DefaultMessage为True或BlockModeVariantAvailable为False；
或本周期列车由定位转为失位状态；
或当前使用的BM信标方向与已定位的列车运营方向TrainFrontOrientation不同。
否则，如果本周期更新BM信标，则将该变量的初始值设置为1（因为ATP使用的是上个周期读到的信标信息）。
其他情况，累加该变量。
```
	def BMbeaconReadAge(k):
	    if (Initialization
	        or not BMvariantValidWhileTemporallyValid(k)
	        or BMbeaconReadAge(k-1) > (ATPsetting.VariantsBMfullValidityTime - 1)
	        or (BeaconVariantsUpdating(k)
	            and (DefaultMessage(k) or not BlockModeVariantAvailable(k)))
	        or (TrainLocalized(k-1)
	            and (not TrainLocalized(k)
	                  or TrackMap.BmBeaconDirection(UsedBMbeaconId(k-1))
	                     is not TrainFrontOrientation(k-1)))):
	        return REPORT_AGE_MAX
	    elif (BeaconVariantsUpdating(k)):
	        return 1
	    else:
	        return BMbeaconReadAge(k-1) + 1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0168]
[End]
[iTC_CC_ATP-SwRS-0147]
BMbeaconVariants[MAX_BM_VARIANT_NB]，存储BLOCK模式下的变量，每个数组元素的结构为ST_BM_VARIANT.
更新规则如下：
如果BMbeaconReadAge大于ATPsetting.VariantsBMfullValidityTime（即为默认值），则设置所有变量BMbeaconVariants为限制状态，认为BM变量无效；
否则，如果本周期BeaconVariantsUpdating为True，则根据线路地图中相应的BM信标，更新每个变量的ValidityTime，LineSection和Index，并使用BeaconMessage.Variants更新变量状态。对于未在该信标中更新的变量，应设置为限制状态。
否则，BM信标变量保持不变。
The structure of array ATP stored BMbeaconVariants are ST_BM_VARIANT. The rules to update the BM variants are as follows:
if the BMbeaconReadAge is larger than the ATPsetting.VariantsBMfullValidityTime, ATP shall set all BM variants as restricted status.
Else If the BeaconVariantsUpdating is True, then ATP update the BMbeaconVariants by the new beacon.
Otherwise, keep BMbeaconVariants unchanged.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0163], [iTC_CC-SyAD-0166], [iTC_CC-SyAD-0167], [iTC_CC-SyAD-0168], [iTC_CC-SyAD-0169], [iTC_CC-SyAD-0170], [iTC_CC-SyAD-0297], [iTC_CC-SyAD-0299], [iTC_CC-SyAD-0841], [iTC_CC_ATP_SwHA-0042], [iTC_CC_ATP_SwHA-0043]
[End]
NOTES:
假设某BM信标是ATP定位使用的第二个信标，而该BM信标的变量方向与列车实际定位方向相反。由于软件功能模块执行顺序的原因，当ATP获取该BM信标的变量信息时，可能还未判断出列车行驶方向（此时尚未执行到定位模块），因此仍然会存储该信标中的变量及其更新有效期。但在执行EOA计算时，会按照列车运行方向（此时已执行完成了定位模块）向下游搜索限制点。但由于存储的变量方向不同，所有列车运行方向下游的带变量奇点均为限制状态，因此上述处理不会影响安全。当下一个周期，ATP发现该BM信标的变量方向与运行方向不符，将其清除。
There is a situation that ATP read one BM beacon as the second beacon for ATP initializing location, and the direction of this BM beacon is opposite with the train movement.
[iTC_CC_ATP-SwRS-0618]
BMbeaconVariantValue，获取来自BM信标中该变量的值，输入索引和周期，若过期为假值
```
	def BMbeaconVariantValue(lineSection, VarIndex, k):
	    if (BMbeaconReadAge(k) > ATPsetting.VariantsBMfullValidityTime):
	        return False
	    else:
	        for Var in range(0, MAX_BM_VARIANT_NB):
	            if (BMbeaconVariants[Var].LineSection == LineSection
	                and BMbeaconVariants[Var].Index == VarIndex):
	                return BMbeaconVariants[Var].Value
	            else:
	                continue
	        else:
	            return False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0295]
[End]
[iTC_CC_ATP-SwRS-0148]
UsedBMbeaconId用于记录当前所使用的BM变量来自哪个BM信标，判断条件如下：
当初始化，非使用BM变量（not BMvariantValidWhileTemporallyValid），该信标方向与当前车头方向不符，或列车失位时，清除UsedBMbeaconId；
否则，如果收到有效的BM信标，记录该信标id到UsedBMbeaconId；
否则，保持UsedBMbeaconId不变。
UsedBMbeaconId records the used BM variants came from which BM beacon:
When one of the following conditions fulfilled, ATP clear the UsedBMbeaconId:
initialization,
the BLOCK MODE variant is not temporally valid,
the direction of the used BM beacon is not as same as train front orientation,
the train is not localized.
Or else:, when received a valid BM beacon, ATP update UsedBMbeaconId;
Otherwise, keep this value unchanged.
```
	def UsedBMbeaconId(k):
	    if (BeaconVariantsUpdating(k)):
	        return BeaconMessage.Id
	    elif (Initialization
	           or not BMvariantValidWhileTemporallyValid(k)
	           or (TrainLocalized(k-1)
	               and (TrackMap.BmBeaconDirection(UsedBMbeaconId(k-1))
	                    is not TrainFrontOrientation(k-1)))
	           or (TrainLocalized(k-1) and not TrainLocalized(k))):
	        return None
	    else:
	        return UsedBMbeaconId(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0163]
[End]
