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