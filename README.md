#UnityTemplate
---
부트캠프 과제 제출용으로 제작하면서 이후 개인 프로젝트에 사용하기 위해 **각 Scene을 Layout(LayoutRoot + Element) + Entity 구조로 분리**, **StreamingAssets 내 JSON 파일과 리소스**를 통해 UI/애니메이션/리소스를 조립하고 소스 설계 말단에 있는 각 Scene에서 요소들에 대해 핸들러를 inject하는 부분만 작업하면 되는 **모딩/확장 친화 프레임워크**를 개발하는 프로젝트

Layout 구조 구현이후 과제 제출 기한에 맞춰 최소한의 기능 추가를 통해 'ForSubmit' 브랜치에서 과제 사양 구현

이후 'master' 브랜치에 병합 완료

추가 기능/확장 설계는 'develop' 브랜치에서 계속할 예정

---

## 구조 요약

- 'LayoutBuilder': JSON 기반 UI 구성(LayoutRoot + Element 기반으로 Layout을 생성)
- 
- 'LayoutInjector': Scene 내 요소들에 핸들러 등록
- 
- 'AnimationPlayer': object내 spriteRenderer 이용해 애니메이션 재생
- 
- 'GameContext': 핸들러, 이벤트 큐 등 전체 문맥 관리

