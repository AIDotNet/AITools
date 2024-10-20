# AITools

AITools 是一个使用 Avalonia 开发的智能 AI 助手。该开源项目利用 .NET 8 框架，旨在提供一个多功能的 AI 驱动的对话和功能平台。目前该项目支持 OpenAI 和 Azure OpenAI 会话，计划在未来集成更多功能，例如本地模型会话（Ollama/Llama）、基于文档的 RAG（检索增强生成）搜索、功能插件等。

## 功能

### 已完成的功能

- [x] **OpenAI 会话**：与 OpenAI 提供的 AI 模型进行对话。
- [x] **Azure OpenAI 会话**：利用微软的 Azure OpenAI 服务进行 AI 驱动的互动。

### TODO

- [ ] **本地模型会话**：支持本地模型，如 Ollama/Llama。
- [ ] **文档上传以进行 RAG 搜索**：上传文档以启用 RAG，从而提供更具上下文的 AI 响应。
- [ ] **功能插件**：通过可定制的插件扩展 AITools 的功能。
- [ ] **语音采集**：麦克风采集发送大模型
- [ ] **多模态模型支持**

## 安装

安装 AITools 之前，请确保你具备以下前提条件：

- .NET 8 SDK

克隆仓库并构建项目：

```
git clone https://github.com/snake-L/AITools.git
cd AITools
dotnet build
```

运行应用程序：

```
dotnet run
```

## 使用

一旦应用程序运行起来，你就可以开始与 AI 模型进行交互。选择所需的 AI 服务（OpenAI 或 Azure OpenAI）并开始你的会话。用户界面设计简洁直观，使 AI 对话变得顺畅。

## 贡献

欢迎对 AITools 进行贡献！如果你有功能建议、改进或漏洞修复，请提交拉取请求或在 GitHub 上创建问题。

1. Fork 本仓库。
2. 创建一个新分支（`git checkout -b feature-branch`）。
3. 进行你的修改。
4. 提交你的修改（`git commit -m 'Add new feature'`）。
5. 推送到分支（`git push origin feature-branch`）。
6. 创建一个拉取请求。

## 许可证

AITools 根据 MIT 许可证发布。详情请参阅 LICENSE 文件。

## 联系方式

如有任何问题或需要支持，请在 GitHub 上创建问题或联系项目维护者：867824092@qq.com。