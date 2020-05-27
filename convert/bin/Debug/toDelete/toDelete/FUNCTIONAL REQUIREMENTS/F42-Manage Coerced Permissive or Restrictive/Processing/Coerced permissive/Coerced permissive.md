﻿
在项目上可以将线路上的部分区域（或点）设置“强制允许”属性，使得ATP判断在满足相应条件下，认为该区域（或点）为“强制允许”状态。“强制允许”的优先级要高于“非强制限制”和该区域（或点）对应的变量状态。可配置的“强制允许”属性如Table 58所示。
Table 58 Configurable coerced permissive identification

|Not Coerced Restrictive|Functional Description|
|-----|
|VARIANTS_OVERLAP_PERMISSIVE|用于在BM模式下强制建立信号机的Overlap|
|COERCED_PERMISSIVE_1|项目配置的强制允许组合输入1|
|COERCED_PERMISSIVE_2|项目配置的强制允许组合输入2|
|COERCED_PERMISSIVE_3|项目配置的强制允许组合输入3|
|COERCED_PERMISSIVE_4|项目配置的强制允许组合输入4|

[iTC_CC_ATP-SwRS-0682]
CoercedPermissive_1，强制允许输入1
```
	def CoercedPermissive_1 (k):
	    CoercedPermissive_1 = Offline.GetCoercedPermissive_1(k)
	    return CoercedPermissive_1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1276], [iTC_CC_ATP_SwHA-0257]
[End]
[iTC_CC_ATP-SwRS-0683]
CoercedPermissive_2，强制允许输入2
```
	def CoercedPermissive_2(k):
	    CoercedPermissive_2 = Offline.GetCoercedPermissive_2(k)
	    return CoercedPermissive_2
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1276], [iTC_CC_ATP_SwHA-0257]
[End]
[iTC_CC_ATP-SwRS-0684]
CoercedPermissive_3，强制允许输入3
```
	def CoercedPermissive_3(k):
	    CoercedPermissive_3 = Offline.GetCoercedPermissive_3(k)
	    return CoercedPermissive_3
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1276], [iTC_CC_ATP_SwHA-0257]
[End]
[iTC_CC_ATP-SwRS-0685]
CoercedPermissive_4，强制允许输入4
```
	def CoercedPermissive_4(k):
	    CoercedPermissive_4 = Offline.GetCoercedPermissive_4(k)
	    return CoercedPermissive_4
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1276], [iTC_CC_ATP_SwHA-0257]
[End]
[iTC_CC_ATP-SwRS-0686]
CoercedPermissive，返回采集到的“强制允许”结果
```
	def CoercedPermissive(cr, k):
	    if (cr is COERCED_PERMISSIVE_1):
	        CoercedPermissive = CoercedPermissive_1(k)
	    elif (cr is COERCED_PERMISSIVE_2):
	        CoercedPermissive = CoercedPermissive_2(k)
	    elif (cr is COERCED_PERMISSIVE_3):
	        CoercedPermissive = CoercedPermissive_3(k)
	    elif (cr is COERCED_PERMISSIVE_4):
	        CoercedPermissive = CoercedPermissive_4(k)
	    elif (cr is VARIANTS_OVERLAP_PERMISSIVE):
	        CoercedPermissive = OverlapTimerPermissive(k)
	    else:
	        CoercedPermissive = False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1187], [iTC_CC-SyAD-1276], [iTC_CC_ATP_SwHA-0257], [iTC_CC_ATP_SwHA-0255]
[End]