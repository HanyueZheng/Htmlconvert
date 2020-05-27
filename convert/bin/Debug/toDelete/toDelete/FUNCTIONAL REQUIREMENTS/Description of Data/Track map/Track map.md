
车载ATP运行时使用的线路地图来自项目配置。如Figure 51所示，线路地图以block为单位组织，由ZC分组管理；在block上，有一系列奇点，分别表示信号机、道岔、站台等实际的物理设备，以及永久限速、保护区等虚拟限制区域或限制点。
The track map onboard ATP used come from project configuration. As shown in Figure 51, the basic constitution unit of track map is block, which managed by ZC; on the block, a series of singularities represent physical devices such as signal, switcher, platform, as well as permanent speed restrictions, protected areas and other virtual restricted zones or limit points.
Figure 51 Track map layout for project
按照线路地图的设计，每个BLOCK上的坐标是相对于该BLOCK上行方向起始点的距离值，如Figure 52所示。。上行方向的BLOCK起点坐标为0，并依次递增直到BLOCK长度。BLOCK下行方向的起点坐标是该BLOCK的最大值，即该BLOCK的长度。如果向下行方向运行，坐标逐渐减小，直到0为止。就是说，在同一个BLOCK上，越往上行坐标越大，反之亦然。如果一个坐标值超过某BLOCK长度，则实际位置应当在该BLOCK上行方向的下游BLOCK上；反之如果坐标小于0，则实际位置应当在该BLOCK下行方向的下游BLOCK上。
According to the design of track map, an abscissa of a block means the distance from the block endpoint of the UP orientation, as shown in Figure 52. The abscissa of the block starts from zero, the UP orientation endpoint, and increases along the UP orientation until reached the length of this block, the DOWN orientation endpoint. That is, in a same block, towards UP orientation, the larger of the abscissa value, the upper of the location; and vice versa. If an abscissa exceeds the length of the block, the actual location should be in the downstream block on the UP orientation; other hand, if an abscissa is less than zero, it should be in the downstream block on the DOWN orientation.
Figure 52 Abscissa increasing rule in block
线路的上行和下行方向，由项目而定。
The UP or DOWN orientation in track map is defined by project.
对于线路上的部分奇点，其状态是会发生动态变化的，例如道岔的位置，信号机为允许或限制等。对于此类奇点，在线路地图中会标有指定的变量作为其状态变化的索引。而这些变量，会通过轨旁设备发送给车载ATP：在CBTC模式下，ATP使用来自ZC发送的变量；而在Block模式下，ATP使用来自BM信标发送的变量，或者使用来自CBI发送的无线变量（如果该项目有无线通信的Block模式）。变量的索引定义如Table 53所示。
Some of the singularities, there status will change dynamically, such as switch positions, permissive or restrictive signals. 
Table 53 Index of variant

|Identification|Logical Type|Description|
|-----|
|Variant|
||LineSec|Variant index in line section of ZC|
|||Id| REF NUMERIC_32 \h NUMERIC_32|The line section id|
|||Index| REF NUMERIC_32 \h NUMERIC_32|The index in line section|
||Cbi|Variant index in CBI for block mode with radio|
|||Id| REF NUMERIC_32 \h NUMERIC_32|The CBI id|
|||Index| REF NUMERIC_32 \h NUMERIC_32|The index in Cbi|

