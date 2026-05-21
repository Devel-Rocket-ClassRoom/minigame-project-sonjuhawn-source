# Fork GUI 기반 두 PC 작업 워크플로

> 집 PC ↔ 학원 PC 사이에서 작업을 주고받기 위한 실전 가이드.
> Fork.dev GUI 기준. 터미널 명령어는 참고용으로 같이 적음.

---

## 1. 저장소 구조

```
upstream (원본 저장소) ──┐
                        │ fetch만 (보통 push 권한 없음)
                        ▼
origin (내 포크) ◀──── 집 PC / 학원 PC 둘 다 push/pull
```

**두 PC 사이 작업 동기화는 항상 `origin`(내 포크)을 통한다.**
upstream은 1주차 끝나고 main 동기화할 때 정도만 신경 쓰면 됨.

### Fork에서 원격 확인
좌측 사이드바 **Remotes** 펼치기 → `origin` URL이 내 포크인지 확인.

---

## 2. PC 옮길 때 기본 흐름

### A. 떠나는 PC에서 (작업 종료 시)
1. **Local Changes** 클릭 — 변경된 파일 확인.
2. 모두 **Stage** (체크 또는 우클릭 → Stage).
3. 커밋 메시지 입력 → **Commit N Files**.
4. 상단 툴바 **Push** → Remote: `origin`, Branch: 현재 작업 브랜치 → Push.
5. 좌측 사이드바 브랜치 옆의 **↑N 화살표가 사라졌는지 확인.**

### B. 도착한 PC에서 (작업 시작 시)
1. 상단 툴바 **Fetch** — 원격 최신 정보 받기.
2. 좌측 사이드바 **Branches**에서 작업 브랜치 옆 화살표 확인.
3. **Local Changes** 클릭 — 안 커밋된 거 있는지 확인.
4. 상태에 따라 아래 시나리오 중 하나로 처리.

---

## 3. 시나리오별 처리

### ✅ 시나리오 C — 깨끗한 상태 (가장 좋음)
- ↑↓ 화살표 없음, Local Changes 비어있음.
- 작업 브랜치 우클릭 → **Pull** (또는 상단 Pull 버튼).
- 끝.

### ⚠️ 시나리오 A — 로컬에 푸시 안 된 커밋이 있음 (↑N 표시)
**원인**: 떠나는 PC에서 커밋만 하고 푸시를 안 했음.

1. 해당 브랜치 우클릭 → **Push** (또는 상단 Push 버튼).
2. 다이얼로그에서 `origin` + 브랜치명 확인 → Push.
3. 그 다음 다른 PC에서 작업한 게 있을 수 있으니 **Pull**도 한번.
4. 분기된 상황이면 Pull 시 머지 또는 리베이스 다이얼로그 뜸:
   - **Merge** — 머지 커밋 생김. 안전.
   - **Rebase** — 히스토리 깔끔. 분기 적을 땐 추천.

### ⚠️ 시나리오 B — 안 커밋된 변경분이 있음
**원인**: 떠나는 PC에서 작업 중 저장만 하고 커밋 안 함.

**옵션 1 — Stash로 임시 보관 후 받기 (추천)**
1. 메뉴 **Repository → Stash → Stash All Changes** (또는 사이드바 Stashes 우클릭).
2. 스태시 이름 입력 (예: `academy WIP`) → 확인.
3. 작업 브랜치 우클릭 → **Pull**.
4. 좌측 **Stashes** 섹션에서 만든 스태시 우클릭 → **Apply Stash**.
5. 충돌 나면 → 시나리오 D 참조.

**옵션 2 — 그냥 커밋부터**
1. Local Changes에서 파일 Stage → 커밋 메시지 → **Commit**.
2. **Push**.
3. 그 다음 **Pull**.

### 🔥 시나리오 D — 머지 충돌 발생
**증상**: Pull 또는 Stash Apply 시 충돌 파일이 빨간색으로 표시됨.

1. Local Changes에서 충돌 파일 우클릭 → **Open in Merge Tool**.
   - 외부 머지툴 없으면 텍스트 에디터로 직접 열기.
2. 파일 안의 충돌 마커 정리:
   ```
   <<<<<<< HEAD
   집 PC 작업
   =======
   학원 PC 작업
   >>>>>>> origin/Feature/...
   ```
   둘 중 하나 또는 둘 다 합쳐서 마커 라인 제거.
3. 정리 후 Local Changes에서 파일 우클릭 → **Stage**.
4. 메뉴 **Repository → Continue Merge** (또는 그냥 커밋 — Fork가 자동으로 머지 커밋 생성).

---

## 4. Unity 프로젝트 특화 주의사항

### `.meta` 파일
- 기존 파일 **수정만** 했으면 `.meta`는 보통 안 바뀜 → staged 안 돼도 OK.
- 새 파일 **생성/이동/이름변경**했으면 `.meta`도 같이 staged 돼야 함.
- 빠지면 다른 PC에서 GUID 깨져서 prefab 참조가 끊김 → 가장 흔한 사고.
- **확인 방법**: Local Changes에서 Unstaged 영역에 `.meta` 파일이 남아있나 보기.

### `.controller` / `.prefab` / `.unity`
- 이 파일들은 YAML 텍스트라서 충돌 가능. 다만 사람이 직접 머지하기 어려움.
- **권장**: 두 PC에서 동시에 같은 prefab/씬/Animator 수정 피하기.
- 충돌 나면 보통 한쪽 통째로 채택 (`Open Mine` 또는 `Open Theirs`)이 안전.

### `Library/`, `Temp/`, `Logs/`, `obj/`
- `.gitignore`에 들어가야 함. 안 들어가 있으면 빌드 캐시까지 다 올라감.
- 이 폴더는 다른 PC에서 Unity 처음 열면 자동 재생성됨.

### `ProjectSettings/`
- InputSystem 액션, 태그/레이어 추가하면 여기 변경됨.
- 빠뜨리면 다른 PC에서 입력/렌더링 다르게 동작. 꼭 커밋.

---

## 5. 푸시 안 받아질 때 (`rejected`)

**증상**: Push 시 "rejected, non-fast-forward" 또는 "behind remote" 에러.

**원인**: 다른 PC에서 같은 브랜치에 푸시한 커밋이 origin에 있는데 내가 그걸 안 받음.

**해결**:
1. 우선 **Pull** 또는 **Fetch + Rebase** 해서 원격 변경분 받기.
2. 충돌 없으면 자동 머지 → 다시 **Push**.
3. 충돌 있으면 → 시나리오 D.

**금지 사항**: `Force Push`는 누가 권한 있어도 **두 PC 사이에서는 절대 쓰지 마세요**. 다른 PC의 커밋을 통째로 덮어쓸 수 있음.

---

## 6. 진짜로 뭔가 잃은 것 같을 때

### 안전망 1 — Reflog
터미널에서:
```bash
git reflog
```
HEAD가 움직인 모든 기록이 나옴. 잘못 reset/checkout해서 사라진 것처럼 보이는 커밋도 거의 다 reflog 해시로 복구 가능.

```bash
git checkout <reflog 해시>   # 일단 그 시점으로 가보기
git branch recovery <reflog 해시>   # 그 시점에서 복구 브랜치 만들기
```

### 안전망 2 — Fork의 Repository History
Fork에서 좌측 **All Commits** 클릭하면 모든 브랜치/태그의 커밋이 그래프로 보임.
브랜치에서 끊어진 커밋도 reflog에 남아있으면 여기 어딘가에 보임.

### 핵심 원칙
- **커밋만 했으면 거의 사라지지 않는다.** 푸시 안 했어도 로컬에는 영구 보존.
- **진짜로 위험한 작업**: `git reset --hard`, `Force Push`, `.git` 폴더 삭제.
- 위 작업을 하지 않았다면 reflog로 거의 모든 상황 복구 가능.

---

## 7. PR 시점 (1주차 빌드 후)

작업 브랜치 origin에 다 푸시된 상태에서:

1. GitHub 웹 → 본인 포크 페이지.
2. "Compare & pull request" 버튼 (또는 Pull requests 탭에서 New).
3. base: `upstream/main`, compare: `origin/Feature/Issue#X`.
4. 이슈 자동 연결: 본문에 `Closes #5` 같은 키워드 → 머지 시 이슈 자동 닫힘.
5. (선택) **Draft Pull Request**로 만들면 머지 불가 상태로 열려서 작업 중 표시 가능.

---

## 8. 응급 명령어 모음 (Fork에서 안 될 때 터미널 백업)

```bash
git status                                # 현재 상태
git branch -vv                            # 브랜치별 ahead/behind
git remote -v                             # 원격 확인
git fetch origin                          # 원격 정보만 받기
git push origin <브랜치명>                # 푸시
git pull origin <브랜치명>                # 풀
git stash push -m "WIP"                   # 임시 보관
git stash pop                             # 보관한 거 복구
git reflog                                # HEAD 이동 기록 (복구용)
git log --oneline --all --graph -20       # 최근 그래프 보기
```
