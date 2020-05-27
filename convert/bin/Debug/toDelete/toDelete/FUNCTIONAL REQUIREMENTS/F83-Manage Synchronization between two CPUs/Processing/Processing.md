﻿
如Figure 524所示，同一块VLE-2板上的两套ATP软件，在计算出给VIOM，冗余ATP，ZC和LC的输出结果后，需通过双口RAM交换该结果，并将对方的计算结果和自己的结果合并后发送给相关外部系统。
Refer to Figure 524, the two set of ATP software in the same VLE-2 board will calculate the output results to VIOM, redundant ATP, ZC or LC, and those data will exchanged by DPRAM. Then the output will merge into one pack of data and send to CCNV. 
Figure 524 Manage synchronization between two CPUs
[iTC_CC_ATP-SwRS-0050]
车载ATP软件在每个周期运行时，需要将VLE-2板两个CPU模块生成的安全输出命令合并组成一帧后发送给CCNV，由其转发给VIOM。在每周期生成运算结果后，ATP软件通过双口RAM将运算结果VIOM1VitalOut和VIOM2VitalOut发送给另一个CPU，并获取来自另一个CPU的运算结果TOC_VIOM1VitalOut和TOC_VIOM2VitalOut。
In every cycle, ATP combines the vital outputs generated by itself and from the other CPU into one frame and sends it to CCNV, who will transmit the frame to VIOM. ATP will send VIOM1VitalOut and VIOM2VitalOut to the other CPU through dual-ports RAM, and receive the TOC_VIOM1VitalOut and TOC_VIOM2VitalOut from the other CPU.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0007], [iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0051]
车载ATP软件将VIOM1VitalOut和TOC_VIOM1VitalOut按照ST_VIOM_OUT结构组合成IdenticalVIOM1Out，将VIOM2VitalOut和TOC_VIOM2VitalOut按照ST_VIOM_OUT结构组合成IdenticalVIOM2Out。
Based on ST_VIOM_OUT structure, ATP combines VIOM1VitalOut and TOC_VIOM1VitalOut as IdenticalVIOM1Out, while based on the same structure ST_VIOM_OUT, ATP combines VIOM2VitalOut and TOC_VIOM2VitalOut as IdenticalVIOM2Out.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0007], [iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0579]
IdenticalLocReport，ATP两CPU同步后的发送给ZC的位置报告信息。当两CPU各自计算完成LocReport后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_LocReport，共同组成IdenticalLocReport，再发送给CCNV，由其转发给ZC。IdenticalLocReport的生成规则如下：
ST_LOC_REPORT中除两重SACEM校核字外，其余均采用本CPU的计算结果；
对于VitalChecksum_1，采用CPU1的数据进行计算；
对于VitalChecksum_2，采用CPU2的数据进行计算。
IdenticalLocReport, the location report after merging two CPU’s results. When the two CPUs complete LocReport calculation, need to send each other through the dual-port RAM; and receive the TOC_LocReport from the other. ATP shall combine the two reports as an IdenticalLocReport, according to the following rules:
The variables without vital checksums in ST_LOC_REPORT, shall use the values calculated by itself.
For the VitalChecksum_1, ATP shall calculate using the values from CPU1.
For the VitalChecksum_2, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]
NOTES:
正常情况下，来自CPU1和CPU2的计算结果应当相同。但是，由于CPU1和CPU2分别通过VPB的上下模块获取里程计数据，由于获取时机的不同步（VLE板两个CPU的中断是各自独立的），可能会导致ATP软件在计算列车位移、速度及其定位上的细微差。因此，需ATP上下模块将获取到的VPB输入信息进行同步（SwRS-0590），之后再计算。若计算出的两个模块的输出结果仍然不一致，则接收方将判断出两路校核字计算结果不一致，从而拒绝该消息。
Normally, the calculated results between CPU1 and CPU2 should be the same – if not, the receiver will get the wrong checksum calculation and thus reject the message. However, due to the independent interrupts of two CPUs, the moment ATP software reading the odometer information from VPB’s registers is not simultaneous, which lead to unavoidable biases in movement or speed calculation by the two ATP software. Therefore, ATP need to synchronize the VPB inputs (SwRS-0590) and then calculate with IdenticalLockedOdometer. 
[iTC_CC_ATP-SwRS-0580]
IdenticalVersionReport，ATP两CPU同步后的发送给LC的版本报告信息。
当两CPU各自计算完成VersionFromCCreport后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_VersionReport，组合生成IdenticalVersionReport，再发送给CCNV，由其转发给LC。IdenticalVersionReport的生成规则如下：
ST_VERSION_REPORT中除安全校核字外，其余变量均采用本CPU的计算结果；
对于VitalChecksum_1，采用CPU1的数据进行计算；
对于VitalChecksum_2，采用CPU2的数据进行计算。
IdenticalVersionReport, the version report after merging two CPU’s results. When the two CPUs complete VersionFromCCreport calculation, need to send each other through the dual-port RAM; and receive the TOC_VersionReport from the other. ATP shall combine the two reports as an IdenticalVersionReport, according to the following rules:
The variables without vital checksums in ST_VERSION_REPORT, shall use the values calculated by itself.
For the VitalChecksum_1, ATP shall calculate using the values from CPU1.
For the VitalChecksum_2, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0581]
IdenticalCCsyncReport，ATP两CPU同步后的发送给冗余ATP的同步信息。
当两CPU各自计算完成CCsynchroReport后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_CCsyncReport组合生成IdenticalCCsyncReport，再发送给CCNV，由其转发给冗余ATP。IdenticalCCsyncReport的生成规则如下：
ST_SYNCHRO_REPORT中除安全校核字外，其余变量均采用本CPU的计算结果；
对于VitalChecksum_1，采用CPU1的数据进行计算；
对于VitalChecksum_2，采用CPU2的数据进行计算。
IdenticalCCsyncReport, the redundant ATP report after merging two CPU’s results. When the two CPUs complete CCsynchroReport calculation, need to send each other through the dual-port RAM; and receive the TOC_CCsyncReport from the other. ATP shall combine the two reports as an IdenticalCCsyncReport, according to the following rules:
The variables without vital checksums in ST_SYNCHRO_REPORT, shall use the values calculated by itself.
For the VitalChecksum_1, ATP shall calculate using the values from CPU1.
For the VitalChecksum_2, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0588]
IdenticalCIsetting[MAX_CONNECTED_PSD_NB],ATP两CPU同步后的发送给联锁PSD控制命令。
当两CPU各自计算完成CIsetting后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_CIsetting组合生成IdenticalCIsetting，再发送给CCNV，由其转发给冗余ATP。IdenticalCIsetting的生成规则如下：
ST_CI_SETTING除校核字外的变量，采用本CPU的计算结果；
对于FSFB2通信协议中的校核字1，采用CPU1的数据进行计算；
对于FSFB2通信协议中的校核字2，采用CPU2的数据进行计算。
IdenticalCIsetting, the PSD control message after merging two CPU’s results. When the two CPUs complete CIsetting calculation, need to send each other through the dual-port RAM; and receive the TOC_CIsetting from the other. ATP shall combine the two reports as an IdenticalCIsetting, according to the following rules:
The variables in ST_CI_SETTING, shall use the values calculated by itself.
For the checkword 1 in FSFB2 protocol, ATP shall calculate using the values from CPU1.
For the checkword 2 in FSFB2 protocol, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0746]
IdenticalCBIvariantRequest，ATP两CPU同步后的发送给联锁的变量请求信息。
当两CPU各自计算完成CBIvariantRequest后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_CBIvariantRequest组合生成IdenticalCBIvariantRequest，再发送给CCNV，由其转发给联锁。IdenticalCBIvariantRequest的生成规则如下：
除安全校核字外，均采用本CPU的计算结果；
对于VitalChecksum_1，采用CPU1的数据进行计算；
对于VitalChecksum_2，采用CPU2的数据进行计算。
IdenticalCBIvariantRequest, the CBI variants request after merging two CPU’s results. When the two CPUs complete CBIvariantRequestcalculation, need to send each other through the dual-port RAM; and receive the TOC_CCvariantReport from the other. ATP shall combine the two reports as an IdenticalCBIvariantRequest, according to the following rules:
The variables without vital checksums, shall use the values calculated by itself.
For the VitalChecksum_1, ATP shall calculate using the values from CPU1.
For the VitalChecksum_2, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]
[iTC_CC_ATP-SwRS-0747]
IdenticalCCvariantReport，ATP两CPU同步后的发送给联锁的Overlap解锁信息。
当两CPU各自计算完成CCvariantReport后，需将其通过双口RAM发送给对方；并接收来自对方的TOC_CCvariantReport组合生成IdenticalCCvariantReport，再发送给CCNV，由其转发给联锁。IdenticalCCvariantReport的生成规则如下：
除安全校核字外，均采用本CPU的计算结果；
对于VitalChecksum_1，采用CPU1的数据进行计算；
对于VitalChecksum_2，采用CPU2的数据进行计算。
IdenticalCCvariantReport, the CC overlap releasable report after merging two CPU’s results. When the two CPUs complete CCvariantReportcalculation, need to send each other through the dual-port RAM; and receive the TOC_CCvariantReport from the other. ATP shall combine the two reports as an IdenticalCCvariantReport, according to the following rules:
The variables without vital checksums, shall use the values calculated by itself.
For the VitalChecksum_1, ATP shall calculate using the values from CPU1.
For the VitalChecksum_2, ATP shall calculate using the values from CPU2.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0005]
[End]