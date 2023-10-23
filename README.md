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

## 关于卡牌效果

> @Kevin

1. 记得在DWall预制体中的脚本和player中的脚本面板将enemyLayer设置为敌人所在的图层！
1. 摧城会爆死垃圾堆周围圆形范围距离1的敌人，调整时直接调整explosion radius（DWall预制体中的脚本面板）
1. 升级剑会攻击前方45度角、半径3 扇形范围内的敌人，调整角度和半径通过player脚本面板最下面的Attack angle 和distance。 **注意**：angle要用度数（360°的那个度）而不是弧度
1. 普通箭的最大飞行距离请在ArrowController里面的Destroy语句，把后面的常数修改（这个常数的意思是，这个对象从创建之时开始，几秒后会自行销毁）

## 关于音效

> @Kevin

1. 主BGM在MyCamera预制体
2. 捡卡音效在CardManager脚本中，记得在挂载该脚本的对象中加入组件AudioSource
3. 其他音效均在player预制体中，不用管



