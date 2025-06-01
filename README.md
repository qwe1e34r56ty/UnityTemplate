# UnityTemplate

Unity의 리빌드 스트레스, Monobehaviour 간의 예측 불가능한 초기화 순서, .unity 파일 충돌 문제를 완전히 문서 기반 런타임 구조로 극복하기 위해 설계한 프레임워크입니다.

## 개요

이 프로젝트는 Unity 개발 중 반복적으로 마주치는 다음과 같은 문제들을 해결하기 위해 시작되었습니다:

- **사소한 변경에도 리빌드/리로딩이 강제되는 비효율적인 개발 흐름**  
- **`.unity` 파일 기반의 충돌 유발 및 병합 불편 문제**  
- **`MonoBehaviour`의 초기화 순서 불확실성과 암묵적인 실행 흐름**
- **객체 간 Update 호출 순서가 명시적이지 않아 디버깅이 어려움**

이러한 문제를 근본적으로 제거하기 위해, 본 프레임워크는 다음과 같은 철학을 기반으로 설계되었습니다:

- 모든 Scene, Entity, Stat, Action은 **외부 JSON 문서로 정의**
- Unity Editor는 최소한으로 사용되며, **GameManager, EventSystem과 Camera만 Scene에 존재**
- 오브젝트 구성과 흐름은 전적으로 **런타임 시점에서 JSON 기반으로 빌드**
- 코드 수정 없이도 JSON만 수정하면 **즉시 반영 가능**하며, **협업과 모딩에 최적화**

## 주요 특징

- ⚙️ **Editor-Free 설계**  
  GameManager를 제외한 모든 오브젝트는 명시적으로 초기화되며, MonoBehaviour의 암묵적인 라이프사이클에서 벗어나 모든 흐름을 코드 기반으로 제어합니다.

- 📄 **StreamingAssets 기반 구조**  
  Scene, Entity, Layout, Animation 등 모든 리소스를 JSON과 이미지 파일로 구성하여 Unity 에디터 의존도를 최소화했습니다.

- 🧱 **Command 패턴을 통한 흐름 제어**  
  오브젝트 생성, 파괴, 씬 전환은 명시적인 커맨드 객체(`BuildEntityCommand`, `ConvertSceneCommand` 등)로 처리되어 디버깅과 테스트가 명확합니다.

- 🔗 **Action-Stat 기반 Entity 구조**  
  엔티티는 껍데기이며, ID 기반으로 Action과 Stat이 연결되어 동작합니다. Action만 작성하면 다양한 Entity를 구성할 수 있습니다.

- 🧩 **전략 패턴 기반 파싱 시스템**  
  string 기반 Stat은 타입 파싱 전략(`StatParser`)을 통해 안전하게 변환되며, 제네릭 캐시를 통해 언박싱 비용도 줄였습니다.

- 🚫 **약결합 설계**  
  Action과 Stat은 강하게 연결되어 있지 않아, 누락된 Stat으로 인한 크래시 없이 동작하며, 필요 시 경고 로그로 처리할 수 있습니다.

- ♻️ **모딩 및 확장 용이성**  
  모든 구성 요소가 문서 기반이므로, 빌드 이후에도 JSON만 수정하면 새로운 Entity나 Scene을 만들 수 있어 모딩에 최적화되어 있습니다.
## 구조 개요

```
Assets/
├── Scenes/
│   └── SampleScene.unity        # GameManager와 MainCamera, EventSystem만 존재
├── StreamingAssets/
│   ├── Json/                    # SceneData, EntityData, Layout 정의
│   ├── Sprites/                 # 리소스 이미지
│   ├── Animations/              # 스프라이트 애니메이션 프레임
│   ├── Entities/                # Entity 정의용 JSON
│   └── TextMeshPro/             # 폰트 리소스
└── Scripts/
    ├── Command/                 # BuildEntityCommand, ConvertSceneCommand 등
    ├── Input/                   # 입력 감지 및 디스패처
    ├── Resource/                # 전략 기반 로더 + 리소스 캐시
    ├── Scene/                   # Scene 로딩 및 구성기
    └── Entity.cs                # Entity 껍데기 클래스
```

## 📚 핵심 아키텍처

### ✈️ Game 흐름 제어
- `ISceneCommand` 그래프: `BuildEntityCommand`, `DestroyEntityCommand`, `ConvertSceneCommand`
- `sceneCommandQueue`: GameContext 내에서 명시적으로 명령 큐 관리

### 📊 리소스 관리
- `IResourceLoaderStrategy<T>` 그래프 + 캐싱
- `ResourceManager.GetResource<T>()` 통해 타입 기반 전략 로딩 (JSON, Texture, Animation 등)

### 🛠 입력 처리 구조
- `MouseInputDetector`, `KeyboardInputDetector`: 입력 수집
- `MouseInputDispatcher`, `KeyboardInputDispatcher`: GameObject 핸들러 트리거 (onClick, onHover 등)

### 🔄 SceneData + EntityData
- `SceneData`: `id` + `EntityTransformData[]`
- `EntityData`: Unity GameObject처럼 설계됨 (`id`, `layer`, `tag`, `transform`, `children`, `actions`, `stats`)

### 🏛️ Entity 실행
- `EntityBase`: string 기반 stat만 보유 (외부 직접 접근 불가)
- `StatParser`: 전략 패턴 기반 파싱 시스템 (캐싱 최적화 포함)
- `Entity`: stat + IAction 리스트 조합으로 동작 수행
- 약결합 설계: stat 누락 시에도 크래시 없이 진행, 필요 시 경고 로그

## ⚙️ 사용 방법

1. Unity 2022.3 이상으로 프로젝트 열기
2. SampleScene.unity 실행 (MainCamera + GameManager만 포함)
3. `StreamingAssets/Json/SceneData.json` 기반으로 초기 화면 자동 구성
4. 새로운 Scene, Entity를 만들고 싶을 경우 JSON 문서만 수정하거나 추가

## 🚀 목표 및 의의

- Unity Editor의 반복적이고 비효율적인 작업 제거
- 협업 시 JSON 기반으로 모든 구조 확인 가능
- 빌드 이후 유연한 콘텐츠 확장을 통한 모딩 지원
- Action 로직만 작성하면 Entity/Scene은 문서로만 조합 가능

## 📃 예시: EntityData 구조

```json
{
  "id": "GoMainButton",
  "layerName": "UI",
  "tagName": "Button",
  "offsetPosition": { "x": 0, "y": 0, "z": 0 },
  "offsetRotation": { "x": 0, "y": 0, "z": 0 },
  "offsetScale": { "x": 1, "y": 1, "z": 1 },
  "actionWithPriorityArr": [
    { "id": "StartButtonAction", "priority": 0 }
  ],
  "statKeyWithValueArr": [
    { "key": "color", "value": "white" }
  ]
}
```

## 추후 개발 방향

- 📨 **중앙 메시지 허브 설계 (Message Hub)**  
  Entity 간 직접 참조 없이 상호작용할 수 있도록 중앙 메시지 허브를 설계할 예정입니다.  
  MQTT 프로토콜 등을 참고해,  
  메세지를 발행해도 수신 대상이 해당 메시지를 처리할 방법이 없다면 아무런 영향도 미치지 않도록 설계됩니다.  
  이를 통해 **Entity 간 은닉을 보장하고, 약결합 구조를 강화**합니다.

- 🚀 **성능 최적화**  
  리소스 로딩, 명령 큐 처리, 애니메이션 갱신 등 전체 시스템 전반에 대해 최적화를 진행할 계획입니다.
