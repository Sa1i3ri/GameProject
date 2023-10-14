# RE: 霓虹残梦

> 南京蒙特利尔工作室

## 摄像机预制体（MyCamera）

> @Kevin

1. 实现了上下左右方向键控制相机移动，以及有移动边框（不会超出特定范围）
2. 先删除原有Camera，确保只有MyCamera
3. 请下载插件包Cinemachine
4. ！！ 调整相机移动速度：在Main Camera 面板下的脚本组件里面调整
5. ！！ 调整相机移动边框：在CameraConfiner中调整多边形碰撞体即可（这个碰撞体就是相机的边界）
6. ！！ 调整相机镜头大小：在Virtual Camera（注意这玩意在Main Camera点开来的下面）下调整 Lens Otho Size
7. 其他东西都别调了，这里纠缠了太多东西变得很抽象。。

## 图层&标签&排序图层

> @syx @Kevin
>
> 后续有任何关于标签/图层/排序图层的更改，请在这里说明

**PS：注意区分 图层 和 排序图层 ！ 排序图层用来控制显示，图层用来控制碰撞矩阵（在Project Settings -> 2D物理 中打开碰撞矩阵）**

1. 关于标签：Player的标签都用“Player”， Hero标签已删除
2. 关于排序图层：background最下面，用于给地板，剩下的地图布置（如墙）都用TileMap，其他暂时都用default

