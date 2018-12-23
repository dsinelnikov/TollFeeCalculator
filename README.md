# General
This implementation has 3 parts. Core, Infrastructure and App.
### Core
Core contains interfaces, toll fee calculator and rules.
Interfaces:
  - *ITollFeeCalculator* - calculator whicth calculate toll fee for specified vehicle and dates list
  - *ITollFeeRule* - calculator uses rules to calculate toll fee. Core have implementations for some rules whitch is needed to handle all test cases. Package has base implementation - Rule. This implementation supports chain to call rules one by one untill rule cannot handle specefied date. You can use RuleExtensions to configure rules easily.
  - *ITollFeeConfiguration* - contains rules and some parameters to calculate toll fee. Calculator requires configuration for initialization. Core do not have any implementation. Client code should create own implementation. General it should be class with simple getters.
  - *IVehicle* - it's empty interface. Each class which is used for identifing vehicle type should implement it.Core uses this interface to build rules dependent of vehicle type.

### Infrastructure
Infrastructure contains vehicle and configuration implementations. I implemented *Configuration* and *SwedishCityConfiguration*. *SwedishCityConfiguration* contain hardcoded configuretion. In real live I prefere use only *Configuration* implementation and load it content from some storage. It give ability to imlement some configuration UI designer and create new configurations without code changing.
I avoid enum using for vehicle types, because I think it's bad practice for this case. I use int value, it give ability to create new types without code changing (Because *IVehicle* do not have special requirements, we can use anything to identify vehicle). This implementation give ability to add vehicles dynamically.

### App
This part is example, how use this implementation.

# Run Application
  - Open *TollFeeCalculator.sln* in Visual Studio
  - Start application

# Run Tests
  - Open TollFeeCalculator.sln in Visual Studio
  - Click menu Test->Run->All Tests
    