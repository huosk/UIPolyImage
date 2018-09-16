# UIPolyImage

在使用UGUI Image组件显示不规则图片、镂空图片时，Image总是会创建一个四边形网格来显示图形，渲染过程中，GPU需要对完全透明的区域进行计算，这不利于性能的优化，一个解决办法是采用多边形网格显示图形，来减少这种不必要的消耗。

整个方案的实现过程包括以下几个步骤：
1. 生成图集，这里推荐使用 [Texture Packer](https://www.codeandweb.com/texturepacker),这里要求导出 *tpsheet* 格式。
2. 导入图集、生成多边形，这里需要从AssetStore下载TexturePackerImporter（已经包含的项目中）。到导入插件之后，将 tpsheet 文件和图集一起导入项目中，导入之后TexturePackerImporter会自动将图集转换成带多边形的Sprite。
3. 使用UIPolyImage组件替换 Image 组件。（该组件目前只支持 Simple 模式）

如果需要根据图形做射线检测，在 UIPolyImage/Image 组件上添加PolyRaycastFilter 组件。因为该组件需要读取贴图的像素，所以需要将贴图的 readAndWrite 属性勾选。 