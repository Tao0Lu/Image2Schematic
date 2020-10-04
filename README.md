
# Image2Schematic C#重制版
Image2Schematic是一款可以将大多数图片转换为Schematic(MCEdit导出文件)的软件。它源于[0xAA55](https://www.0xaa55.com/) VB 开发开源的 [MapPaintGen](https://www.0xaa55.com/thread-2035-1-1.html)，它还有"凹凸调色板"的伪立体模式。
![Image2Schematic.png](https://i.loli.net/2020/10/04/UcAHs39bqQBjWwp.png)
_图片较大,MCEdit显示会压制_
## 运行环境
  - 足够的内存大小和硬盘大小
  最大时候占用内存大约为
 > 2.35 x 图片宽大小 x 图片高大小 x 生成高度(平坦模式为1，立体模式最多255)
  
  字节
  - .NET框架2.0以上

## 图片大小
图片的宽高最大为64k(65,535)，也就是说图片最大为4096k(4,294,967,295)像素
但是对应的硬件内存也需要支持(McEdit在Schematic文件过大时候内存会溢出)
## 调色板设置
 |模式|详情|
|:--|:--|
|  近似颜色 | 生成较差  |
|  矩形抖动(图案仿色) | 对保留图像形状较好  |
| 误差扩散(扩散仿色) |  对色彩的保留度较好|
