## chat GPT 提供的結構範本
TextAdventure/  
  ├── Application/  
  │   ├── UseCases/  
  │   │   ├── GetStoryUseCase.cs  
  │   │   ├── GetChoicesUseCase.cs  
  │   │   ├── MakeChoiceUseCase.cs  
  │   ├── Interfaces/  
  │   │   ├── IGetStoryInputPort.cs  
  │   │   ├── IGetStoryOutputPort.cs  
  │   │   ├── IGetChoicesInputPort.cs  
  │   │   ├── IGetChoicesOutputPort.cs  
  │   │   ├── IMakeChoiceInputPort.cs  
  │   │   ├── IMakeChoiceOutputPort.cs  
  ├── Domain/  
  │   ├── Entities/  
  │   │   ├── Story.cs  
  │   ├── Interfaces/  
  │   │   ├── IStoryRepository.cs  
  ├── Infrastructure/  
  │   ├── Repositories/  
  │   │   ├── StoryRepository.cs  
  ├── Presentation/  
  │   ├── Console/  
  │   │   ├── Program.cs

專案分層：  
### Domain：  
遊戲的核心，  包含劇情、互動規則等等。  
### AppFlow：
關乎如何在本應用程式進行此遊戲的方式，一些初始化、相互呼叫等等。
### UseCase：
關於使用者怎麼使用這個程式，比方說，一些劇情上的選擇、戰鬥方式等等。
### Adapter：
### Framework：