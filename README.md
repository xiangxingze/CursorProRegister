# CursorPro Register

这是一个基于 [cursor-auto-free](https://github.com/chengazhen/cursor-auto-free) 项目的 .NET 实现版本，用于自动化 Cursor Pro 账号注册流程。

## 功能特点

- 使用 .NET MAUI 构建的现代化跨平台界面
- 基于 Playwright 的浏览器自动化
- 支持临时邮箱验证
- 自动处理 Turnstile 验证
- 实时进度和日志显示

## 系统要求

- Windows 10 或更高版本
- .NET 9.0
- Chrome 浏览器

## 使用方法

1. 下载并运行程序
2. 配置以下信息：
   - 邮箱域名：你的 Cloudflare 域名（例如：example.com）
   - 临时邮箱：tempmail.plus 的邮箱前缀
3. 点击"开始"按钮
4. 等待注册流程自动完成

## 开发环境

- Visual Studio 2022 或更高版本
- .NET 9.0 SDK
- MAUI 开发工具

## 项目结构

```
src/
├── CursorAutoFree.Core/        # 核心业务逻辑
├── CursorAutoFree.UI/         # MAUI 用户界面
└── Infrastructure/           # 基础设施实现（浏览器自动化等）
```

## 构建说明

1. 克隆仓库：
```bash
git clone https://github.com/xiangxingze/CursorProRegister.git
```

2. 进入项目目录：
```bash
cd CursorProRegister
```

3. 构建项目：
```bash
dotnet build
```

4. 运行项目：
```bash
dotnet run --project src/CursorAutoFree.UI
```

## 使用注意事项

1. 运行环境要求：
   - 确保已安装 Chrome 浏览器
   - 稳定的网络连接（建议使用国外节点）
   - 不要开启全局代理

2. 邮箱配置：
   - 需要使用 Cloudflare 域名邮箱
   - 需要配置 tempmail.plus 邮箱
   - 将 Cloudflare 域名邮箱转发到 tempmail.plus 邮箱

## 免责声明

本工具仅供学习研究使用，请遵守相关服务条款。使用本工具产生的任何后果由使用者自行承担。严禁将本工具用于任何商业用途。

## 致谢

本项目基于 [cursor-auto-free]项目进行 .NET 重构，感谢原作者的开源贡献。

## 许可证

MIT License

# Cursor Pro 自动化工具使用说明

README also avaiable in: [English](./README.EN.md), [Tiếng Việt](./README.VI.md)

## 最近有人把软件拿到咸鱼卖的，这种事情尽量少干吧。不至于什么钱都挣。


## 许可证声明
本项目采用 [CC BY-NC-ND 4.0](https://creativecommons.org/licenses/by-nc-nd/4.0/) 许可证。
这意味着您可以：
- 分享 — 在任何媒介以任何形式复制、发行本作品
但必须遵守以下条件：
- 非商业性使用 — 您不得将本作品用于商业目的


## 功能介绍
自动注册账号，自动刷新本地token，解放双手。

## 下载地址
https://github.com/xiangxingze/CursorProRegister/releases



## 重要提示
**1.确保你有一个chrome浏览器；如果你没有；[下载地址](https://www.google.com/intl/en_pk/chrome/)**

**2.首先，你要自己已经登录过账号不管你的账号是不是有效，登录是必须的。**

**3.有一个稳定的网络连接。尽量是国外的节点。不要开启全局代理。一定不要开启全局代理。**

## 配置说明

+ 需要使用 cloudflare 域名邮箱，请自行搜索如何使用 cloudflare 域名邮箱，请自行搜索如何使用。
+ **（非常重要）** 需要使用 tempmail.plus 邮箱，请自行搜索如何使用 tempmail.plus 邮箱。
+ 将 cloudflare 的域名邮箱转发到 tempmail.plus 邮箱。
+ 下载 .env.example 文件到程序所在根目录，并重命名为 .env 文件。


在使用之前，需要配置环境变量文件。

```bash
DOMAIN='xxxxx.me'    # 你的邮箱域名 自行搜索如何使用 cloudflare 邮箱
TEMP_MAIL='xxxxxx'   # 临时邮箱，就是你cloudflare 设置的目标位置 用的还是https://tempmail.plus/zh/#! 的邮箱
```

例如，如果你设置：
```bash
DOMAIN='niceme.me'
TEMP_MAIL='ccxxxxcxx'
```
那么程序将随机生成 `@niceme.me` 后缀作为注册邮箱。


## 运行方法

### Mac 版本
1. 打开终端，进入应用所在目录
2. 运行命令：授权文件可以执行
```bash
chmod +x ./CursorPro
```
3. 运行程序：
   - 在终端中运行：
```bash
./CursorPro
```
   - 或直接在访达（Finder）中双击运行


提示：如果遇到下面的问题; [解决方案](https://sysin.org/blog/macos-if-crashes-when-opening/)


![image](./screen/c29ea438-ee74-4ba1-bbf6-25e622cdfad5.png)



### Windows 版本
直接双击运行 `CursorPro.exe`





## 使用注意事项

1. 运行环境要求：
   - 稳定的网络连接
   - 足够的系统权限

2. 使用过程中：
   - 等待程序自动完成所有操作
   - 看到"脚本执行完毕"提示后再关闭程序
  
## 赞助更有动力更新



## 常见问题解决

1. 程序运行过程中卡住：
   - 检查网络连接
   - 重启程序重试


## 免责声明
本工具仅供学习研究使用，请遵守相关服务条款。使用本工具产生的任何后果由使用者自行承担。严禁将本工具用于任何商业用途，包括但不限于销售、租赁或其他营利行为。违反许可证条款的行为将承担相应的法律责任。

## 更新日志
- **2025-01-09** 增加了 log 日志，方便调试，增加了退出cursor，自动构建功能
- **2025-01-10** 修改为使用 cloudflared 域名邮箱
- **2025-01-11** 增加了可以通过 .env 文件配置 无头模式，增加了代理



