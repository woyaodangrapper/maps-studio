# src

洋葱框架开发目录:

| Sample                           | Description                                                                                                                                                                                    |
|----------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [1. Application Core](./Application%20Core) | 应用核心及领域模型.
| [3. Application Services](./Application%20Services)  | 应用服务.
| [4. Infrastructure](./Infrastructure) | 适配器/外部引用/图形显示等.


**核心**
- Application Core - 应用核心及领域模型

  - Domain - 领域

    - Services - 领域服务

      - Common - 通用域

        - User - 用户

        - DCI - 数字版权 / Digital Copyright Identifier

        - SUS - 软件更新 / Software Update Service

        - HWC - 硬件控制 / HardWare Controller

      - Core - 核心域

        - ML - 机器学习 / Machine Learning

        - AI - 人工智能 / Artificial Intelligence

        - IOT - 物联网 / Internet of Things

      - Support - 支撑域

        - HWCI - 硬件配置 / HardWare Configuration Item

        - CSCI - 软件配置 / Computer Software Configuration Item

        - ACID - 数据库事务处理 / 原子性（atomicity，或称不可分割性）、一致性（consistency）、隔离性（isolation，又称独立性）、持久性（durability）。
    - Entitys - 实体

- Application Services - 应用服务

- Infrastructure - 适配器/外部引用/图形显示等

## 采用设计模式

**领域服务及模型**

- 创建型

  - 建造者模式（Builder Pattern）

- 结构型

  - 享元模式（Flyweight Pattern）
  - 组合模式（Composite Pattern）
  - 桥接模式（Bridge Pattern）

- 行为型

  - 访问者模式（Vistor Pattern）

  - 观察者模式（Observer Pattern）
  
  - 中介者模式（Mediator Pattern）

  - 状态模式（State Pattern）

**应用服务及应用**

- 创建型

  - 抽象工厂模式（Abstract Factory）

- 结构型

  - 装饰者模式（Decorator Pattern）

- 行为型

  - 命令模式（Command Pattern）
