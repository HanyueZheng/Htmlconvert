﻿
同一项目中，运行在不同车辆的CC上的ATP软件本身是相同的。在与外部系统通信时，依靠CC SSID来区分当前的CC标识；同时，ATP还需知道自身所在车头是END_1还是END_2；以及运行在VLE-2板的哪个CPU模块。上述信息均需通过读取安装在VLE-2板上的CC data plug获取。须由操作人员保证，安装在每一块VLE-2板上的CC data plug都是正确的，唯一的。
In the same project, the ATP software running on different vehicles is identical. When communicating with external systems, the ATP depends on the CC SSID to identify itself; meanwhile, it needs to know itself in the train END_1 or END_2; and runs in the CPU1 or CPU2 module of the VLE-2 board. These information are stored in CC data plug where installed on each CPU module of the VLE-2 board. The maintenance staff guarantees the correctness and uniqueness of CC data plug.
[iTC_CC_ATP-SwRS-0053]
初始化时，ATP读取来自CC data plug的DataPlugContent.CCTrainType信息，生成TrainType。
On initialization, ATP generates TrainType according to DataPlugContent.CCTrainType from the CC data plug.
‘’’
```
	def TrainType(k):
	    return DataPlugContent.CCTrainType
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0371]
[End]
[iTC_CC_ATP-SwRS-0054]
初始化时，ATP读取来自CC data plug的DataPlugContent.CCCoreId信息，生成CoreId。
On initialization, ATP generates CoreId according to DataPlugContent.CCCoreId read from the CC data plug.
```
	def CoreId(k):
	    return DataPlugContent.CCCoreId
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0371]
[End]
[iTC_CC_ATP-SwRS-0613]
OtherCoreId，远端车头号
Core id for CC on the distant cab.
```
	def OtherCoreId(k):
	    if (CoreId(k) is END_1):
	        return END_2
	    elif (CoreId(k) is END_2):
	        return END_1
	    else:
	        return None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0371]
[End]
[iTC_CC_ATP-SwRS-0055]
初始化时，ATP读取来自CC data plug的DataPlugContent.CC_SSID信息，生成SubSystemId。
On initialization, ATP generates SubSystemId according to DataPlugContent.CC_SSID from the CC data plug.
```
	def SubSystemId(k):
	    return DataPlugContent.CC_SSID
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0210], [iTC_CC-SyAD-0371]
[End]
