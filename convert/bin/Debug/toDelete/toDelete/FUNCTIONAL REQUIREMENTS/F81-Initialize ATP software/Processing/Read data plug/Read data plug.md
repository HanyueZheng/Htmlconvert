﻿
[iTC_CC_ATP-SwRS-0511]
初始化时，ATP读取带VCP编码的来自CC data plug接口的信息CCdataPlugInfo，生成DataPlugContent，其结构如Table 44所示。
In Initialization, ATP reads the CCdataPlugInfo with VCP coded from CC data plug, and generates DataPlugContent with the structure shown as Table 44。
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0067], [iTC_CC_ATP_SwHA-0185]
[End]
[iTC_CC_ATP-SwRS-0037]
初始化时，ATP软件通过DataPlugContent.VLECpuId来识别所在的是CPU1还是CPU2，并与另一个CPU模块建立通信。
如果从Dataplug读到的cpuId错误，则导致双CPU建立通信失败，ATP软件禁止执行，等待操作人员手动重启系统；
对于CPU1，将在等待双CPU建立通信时控制LED显示WAITING_CPU信息。
对于在CPU1运行的ATP软件，如果读到的cpuId既不是CPU1也不是CPU2时，控制LED显示ERR_CPU_ID信息。
Through DataPlugContent.VLECpuId, ATP recognizes whether it is located in CPU1 or CPU2, and establish the communication between each other.
If the CPU id is wrong, ATP shall stop and wait for reboot manually by the operator. 
For CPU1, ATP shall control the LED to show WAITING_CPU when the communication establishing.
For CPU1, ATP shall control the LED to show ERR_CPU_ID when the cpuId is neither CPU1 nor CPU2.
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source= [iTC_CC-SyAD-0831], [iTC_CC-SyAD-0032], 
[iTC_CC-SyAD-1423][End]
NOTES:
由于硬件设计限制，仅有CPU1的软件可以控制面板的LED显示。
Due to the limitation of hardware, only the software running on CPU1 can control the display of LED in the front panel of VLE-2 board.