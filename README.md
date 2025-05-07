#UnityTemplate
---
부트캠프 과제 제출용으로 제작하면서 이후 개인 프로젝트에 사용하기 위해 **각 Scene을 Layout(LayoutRoot + Element) + Entity 구조로 분리**, **StreamingAssets 내 JSON 파일과 리소스**를 통해 UI/애니메이션/리소스를 조립하고 소스 설계 말단에 있는 각 Scene에서 요소들에 대해 핸들러를 inject하는 부분만 작업하면 되는 **모딩/확장 친화 프레임워크**를 개발하는 프로젝트

Layout 구조 구현이후 과제 제출 기한에 맞춰 최소한의 기능 추가를 통해 'ForSubmit' 브랜치에서 과제 사양 구현
이후 'master' 브랜치에 병합 완료
추가 기능/확장 설계는 'develop' 브랜치에서 계속할 예정

---

## 구조 요약

- 'LayoutBuilder': JSON 기반 UI 구성(LayoutRoot + Element 기반으로 Layout을 생성)
- 'LayoutInjector': Scene 내 요소들에 핸들러 등록
- 'AnimationPlayer': object내 spriteRenderer 이용해 애니메이션 재생
- 'GameContext': 핸들러, 이벤트 큐 등 전체 문맥 관리
- Element, 리소스, Layout 등의 외부 리소스와의 매칭을 위한 상수 ID 클래스들
- Entity 관련 구조는 설계 중

현재 과제 기능을 위해 미니게임 2개 + 기존 개발 중이던 부분을 각 Scene으로 나눠 
PlaneGameScene + PunchGameScene + SampleScene으로 만들고
PlaneGameSceneScript, PunchGameSceneScript는 각 미니게임 관련 스크립트를 가지고 있으며
그 외 스크립트는 기존 개발 중이던 부분 + 간단한 메타버스 구현 + 각 미니게임 Scene으로 넘어가는 부분으로 구성

---

## 브랜치 설명
-'master': 과제 제출용 브랜치
-'ForSubmit': 과제 제출을 위한 기능 구현 
-'develop': 프레임워크 개발용
