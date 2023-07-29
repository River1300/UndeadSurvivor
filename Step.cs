/*
1. 뱀서라이크 - 2D 오브젝트 만들기

    #1. 오브젝트 만들기
        [a]. 아틀라스( 스프라이트 시트 )를 잘라서 사용해야 한다.
            Farmer 0 을 선택한다.
                PixelsPerUnit : 한 칸에 픽셀이 몇개가 들어가는지 ( 18 개로 지정 )
                FilterMode : Point no Filter
                Compression : 색상의 압축 방싱 ( None으로 지정 )
                Sprite Mode : Multiple로 지정
                SpriteEdit에 들어가서 GridByCellSize로 18 / 20으로 Padding은 1 / 1로 지정하여 자른다.
                잘린 스프라이트의 이름을 바꿔준다.
        [b]. 아틀라스에서 자른 스프라이트를 확인하고 Stand 0 스프라이트를 씬에 배치한다.
        [c]. Stand 0을 Player로 명명 한다.
        [d]. 위치 값을 초기화 한다.
        [e]. 기즈모 버튼에서 3D Icons의 크기를 작게 줄여 준다.

    #2. 컴포넌트 추가하기
        [a]. 플레이어는 물리적인 이동을 할 예정이다.
        [b]. 리지드바디 컴포넌트를 추가한다.
            상하좌우로 움직일 예정이므로 중력 값을 0으로 지정한다.
        [c]. 캡슐 콜라이더 컴포넌트를 추가한다.
            Size : 0.6 / 0.9
        [d]. 아틀라스 중 프롭스 아틀라스를 찾아서 그림자 스프라이트를 Player 자식으로 추가한다.
            위치 y축 - 0.45로 지정
            OrderInLayer를 Player는 5, Shadow는 0
        [e]. Main 카메라의 배경 색을 회색으로 수정
*/

/*
2. 뱀서라이크 - 플레이어 이동 구현하기

    #1. C# 스크립트 만들기
        [a]. 스크립트를 모아둘 폴더를 만든다.
        [b]. Player 스크립트를 만든다.

    #2. 키보드 입력 받기
        [a]. 움직이기 위한 입력을 받아야 한다.
            받기 위한 변수가 속성으로 필요 하다.
                public Vector2 inputVec;
        [b]. Update() 함수에서 입력을 받는다.
            inputVec.x 가 수평값을 받는다. ( GetAxis )
            inputVec.y 가 수직값을 받는다.
        [c]. Player 스크립트를 Player 객체에 부착한다.

    #3. 물리 이동 방법
        [a]. 이동을 하기 위해 리지드바디를 사용해야 한다.
            Rigidbody2D rigid;
        [b]. 리지드 바디를 Awake()에서 초기화 한다.
        [c]. 물리에 대해서는 FixedUpdate()에서 로직을 작성하도록 한다.
        [d]. 힘을 준다.
            rigid.AddForce()
        [e]. 속도 제어
            rigid.velocity = 
        [f]. 위치 이동
            rigid.MovePosition()

    #4. 물리 이동 구현
        [a]. 위치 이동을 활용하여 플레이어를 이동 시키도록 한다.
            rigid.MovePosition(rigid.position + inputVec);
        [b]. 컴퓨터 성능과 관계없이 캐릭터 속력을 동일하게 맞추어 준다.
            Vector2 nextVec 이라는 지역 변수를 만들어서 방향 값의 크기를 정규화시킨다.
                = inputVec.normalized * speed * Time.fixedDeltaTime;
            public float speed 속성을 새로 갖는다.
        [c]. rigid.MovePosition(rigid.position + nextVec);
        [d]. 미끄러지듯 움직이는 것을 방지하기 위해 입력값을 받는 로직을 GetAxisRaw로 수정한다.
        [e]. 씬으로 나가서 speed 속성 값을 3으로 지정
*/

/*
3. 뱀서라이크 - 인풋 시스템 적용하기

    #1. 패키지 설치
        [a]. Window -> PackageManager
        [b]. Unity Registry
        [c]. Input System -> Install
        [d]. 새로운 Input System으로 프로젝트를 세팅해야 하므로 재시작을 해야 한다.

    #2. 인풋 액션 설정
        [a]. Player에게 PlayerInput 컴포넌트 추가
        [b]. CreateActions -> Undead 폴더에 Player이름의 새 Actions 생성
        [c]. Move Action을 사용할 예정이다.
            Move를 펼치면 디바이스 별 키 입력이 들어 있다.
        [d]. Processors에서 Normalize Vector2를 추가한다.
        [e]. Save Asset

    #3. 스크립트 적용
        [a]. 네임스페이스로 UnityEngine.InputSystem을 추가 한다.
        [b]. Update() 함수를 제거한다.
        [c]. 씬에서 Player에 부착한 PlayerInput 컴포넌트를 보면 Behavior 아래에 사용 가능한 함수들이 나열되어 있다.
            OnMove()를 사용할 예정이다.
        [d]. 함수 void OnMove()를 만든다.
            매개 변수로 InputValue value를 받는다.
        [e]. inputVec 속성에 값을 저장하도록 한다.
            inputVec = value.Get<Vector2>();
        [f]. 이미 액션 설정에서 Normalize를 추가 하였으므로 FixedUpdate()에서 작성해 두었던 normalized는 필요 없다.
*/

/*
4. 뱀서라이크 - 2D셀 애니메이션 제작하기

    #1. 방향 바라보기
        [a]. 스프라이트 랜더러에 보면 Flip이 있는데 이를 통해 이미지를 반전 시킬 수 있다.
        [b]. Player 스크립트에서 Update()를 통해 입력 값을 받고 있다.
        [c]. LateUpdate() 함수를 만든다.
            만약에 inputVec.x 값이 0이 아닐 경우 스프라이트를 반전 시키도록 한다.
        [d]. 속성으로 스프라이트 랜더러를 받는다.
        [e]. if(inputVec.x != 0) spriter.flipX = inputVec.x < 0;

    #2. 셀 애니메이션
        [a]. 셀 애니메이션이란 여러 장의 이미지를 순차적으로 보여주는 방식을 뜻한다.
        [b]. Run 0 ~ Run 5를 모두 선택한 상태로 Player객체에 넣어 준다.
        [c]. Animations 폴더에 Run_Player0 anim이라는 이름을 만든다.
        [d]. Stand 0 ~ Stand 3 모두 선택한 상태로 Player객체에 넣어 준다.
        [e]. Animations 폴더에 Stand_Player0 anim이라는 이름으로 만든다.
        [f]. Dead 0 ~ Dead 1도 마찬가지...
        [g]. 애니메이션 컨트롤러의 이름을 AcPlayer로 명명

    #3. 애니메이터 설정
        [a]. Default 상태를 Stand_Player0으로 지정한다.
        [b]. 애니메이터에 있는 상태 이름을 Stand, Run, Dead로 명명
        [c]. Dead는 AniState 위로 옮기고 연결한다.
            Dead에서 Exit는 연결하지 않는다.
        [d]. Stand와 Run을 서로 연결한다.
        [e]. 파라미터를 추가한다.
            Float타입의 Speed
            Trigger타입의 Dead
        [f]. Stand <=> Run 에서 Speed를 Greater 0.01, Less 0.01로 지정한다.
        [g]. Dead에 Trigger
        [h]. 셀 애니메이션 이므로 Transition Duration은 0.01로 모두 줄이고 Has Exit Time도 모두 해제
        [i]. Dead 애니메이션은 반복할 필요가 없다.
            Dead 애니메이션의 loop Time을 해제한다.

    #4. 코드 작성하기
        [a]. 애니메이터를 속성으로 받고 초기화를 진행한다.
        [b]. LateUpdate()에서 애니메이션 파라미터를 전달한다.
            Float로 벡터의 크기를 전달한다.
                anim.SetFloat("Speed", inputVec.magnitude);

    #5. 애니메이터 재활용
        [a]. 플레이어 종류가 다양하게 있는데 만들어둔 애니메이션을 재활용 하여 다른 플레이어에게 적용 시키자.
        [b]. 애니메이션 스프라이트는 각기 다르기 때문에 애니메이션 클립으로 각각 저장해 주어야 한다.
            이전 마찬가지로 Animations 폴더에 Player0 ~ 3으로 각각 저장한다.
        [c]. 애니메이터에 추가된 새로운 클립들은 지운다.
        [d]. 기존에 만들어 두었던 AcPlayer가 있다.
        [e]. Animator Override Controller을 만들어서 AcPlayer1로 명명 한다.
            Controller에 AcPlayer을 넣어 준다.
            Override에 해당 애니메이션 클립을 부착한다.
        [f]. 테스트를 위해 Player객체를 복사한다.
            스프라이트는 수정할 필요가 없고 애니메이터 컨트롤러만 바꿔준다.
*/

/*
5. 뱀서라이크 - 무한 맵 이동

    #1. 타일 그리기
        [a]. 스프라이트 폴더에서 Tiles 아틀라스를 통해 맵을 그릴 예정이다.
        [b]. Window -> Tile Palette를 연다.
        [c]. Tile 폴더에서 2D -> Rule Tile을 만든다.
            Ran Tile로 명명
        [d]. Number of Tiling Rules 를 1로
        [e]. Output을 Random으로
        [f]. Size를 10으로 지정한다.
        [g]. Tile 0 ~ 2 각각 두 개씩 배치 3 ~ 5는 하나 씩 배치
        [h]. 완성된 타일 에셋을 팔레트에 드래그 드랍하여 덮어 씌운다.
        [i]. 하이어라키 창에서 2D Object -> TileMap -> Rectangular을 만든다.
        [j]. 타일 팔레트를 선택하면 아래에 Default Brush가 있다.
            Line Brush로 선택한다.
                그릴 시작 지점 클릭 -> 10칸 오른쪽으로 그린다.
                반대쪽도 마찬가지로 총 중심을 기준으로 20칸을 그린다.
                아래로도 10칸 위로도 10칸 그린다.
            Default Brush로 바꾼뒤 채우기 아이콘으로 빈칸을 채운다.
        [k]. Ran Tile에서 Nois 수치를 조절하면서 원하는 형태의 타일맵 제작

    #2. 재배치 이벤트 준비
        [a]. 앞으로 타일맵을 4개로 복사하고 플레이어와의 거리가 멀어진 맵을 가까워진 구역으로 재배치할 예정이다.
        [b]. 재배치를 하기 위해서 맵( 타일 맵 )에 타일맵 콜라이더 2D 컴포넌트를 부착한다.
        [c]. 컴포짓 콜라이더2D를 부착한다.
            타일맵 콜라이더에서 used By Composit을 체크한다.
            컴포짓 콜라이더는 트리거 체크
        [d]. 리지드 바디 Body Type은 Static으로 바꾼다.
        [e]. Tag를 추가한다. Ground, Area
        [f]. TileMap은 Ground 태그를 부착
        [g]. Player 객체에서 자식으로 빈 오브젝트를 추가한다.
            Area로 명명
            Box 콜라이더 추가
                Size 20 / 20
                트리거 체크
                Area 태그 부착

    #3. 재배치 스크립트 준비
        [a]. 타일맵 이동을 위한 스크립트를 만든다. Reposition
        [b]. 게임 매니저 스크립트도 만든다.
            빈 오브젝트를 만들어서 게임 매니저 스크립트를 부착한다.
            게임 매니저를 통해서 각각의 타일에 플레이어 정보를 전달할 예정이다.
        [c]. 게임 매니저는 Player 스크립트 변수를 속성으로 갖는다.
            게임 매니저 자체를 메모리에 얹기 위해 정적으로 게임 매니저 타입의 변수를 속성으로 같는다.
                public static GameManager instance;
                Awake() 함수에서 초기화 한다. instance = this;
                게임 매니저가 필요한 다른 클래스에서 구지 속성으로 게임 매니저를 받지 않아도 instance를 통해 게임 매니저에 접근할 수 있다.

    #4. 재배치 로직
        [a]. Reposition 스크립트에 OnTriggerExit2D 함수를 만든다.
            Player의 자식인 Area와 Exit 상태가 되었을 때 이동하도록 한다.
                if(!other.CompareTag("Area")) return;
        [b]. 타일맵과 플레이어간의 거리를 x, y 별로 비교할 필요가 있다.
            이를 위해 Vector3 변수로 플레이어 위치와 맵의 위치를 저장한다.
                Vector3 playerPos = GameManager.instance.player.transform.position;
                Vector3 myPos = transform.position;
        [c]. x 축 따로 y축 따로 거리를 변수로 저장한다.
            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);
        [d]. 플레이어가 어느 방향으로 가고 있는지 변수로 저장한다.
            Vector3 playerDir = GameManager.instance.player.inputVec;
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;
        [e]. 적들 또한 재배치가 필요하므로 switch 문으로 자신의 transform.tag를 통해 다른 로직을 실행한다.
            Ground일 경우, Enemy일 경우
            만약에 x축의 거리가 y축의 거리 차이보다 더 클 경우 타일맵의 위치를 지정된 값 만큼 현재 위치에서 이동한다.
                if(diffX > diffY) transform.Translate(Vector3.right * dirX * 40);
            y축의 거리 차이가 더 클 경우
        [f]. 씬으로 돌아가서 Tile맵을 배치하도록 한다.
            씬 상단에 Snap Increment을 선택하여 Move를 10으로 지정한다.
            씬의 기즈모 화살표를 ctrl 누른 상태에서 이동 시킨다.
            타일맵을 총 4개로 만들어 준다.

    #5. 카메라 설정
        [a]. 배경의 픽셀이 움직일때 깨지는 현상이 발생하는데 픽셀 퍼펙트를 사용해야 한다.
        [b]. 카메라에 픽셀 퍼팩트 카메라 컴포넌트를 부착한다. URP 버전
            PixelsPerUnit은 18로 지정
            픽셀 퍼팩트 카메라는 해상도가 짝수여야만 한다.
            레퍼런스 레솔루션 : 카메라 크기 계산을 위한 참고 해상도 ( 135 / 270 )
        [c]. 씬 상단에 있는 Game을 Simulate로 바꾼다.
            기기마다 출력되는 화면을 미리 볼 수 있다.
        [d]. Window -> PackageManager에서 Unity Registry에서 Cinemachine을 설치한다.
        [e]. 하이어라키 창에서 씨네머신 -> 버츄얼 카메라를 생성한다.
            Virtual Camera로 명명
        [f]. 버츄얼 카메라에서 Follow에 Player객체를 넣는다.
        [g]. 플레이어 이동이 매끄럽지가 않은데 Update의 타임 때문이다.
            메인 카메라 컴포넌트인 씨네머신 브레인에서 Update Method를 FixedUpdate로 바꾼다.
        [h]. 플레이어 그림자를 보이기 위해서 TileMap의 Order Layer을 -1로 지정한다.
*/

/*
6. 뱀서라이크 - 몬스터 만들기

    #1. 오브젝트 만들기
        [a]. 스프라이트 폴더에 5가지의 몬스터 아틀라스가 준비되어 있다.
            오브젝트 형태를 만든다.
            Enemy 1의 Run 0을 하이어라키 창에 놓는다.
            Enemy로 명명
            OrderInLayer는 2로 지정한다.
            플레이어와 마찬가지로 그림자 스프라이트를 자식으로 등록한다.
                y축만 -0.45
        [b]. 애니메이터 컴포넌트 추가 및 컨트롤러 연결
            AcEnemy 1을 컨트롤러에 연결
        [c]. 리지드바디를 부착한다.
            중력 0, FreezeRotation z축 체크
        [d]. 캡슐 콜라이더를 부착한다.
            Size 0.7 / 0.9
        [e]. Enemy 객체를 복사한다.
            Enemy2 의 스프라이트를 넣어 준다.
            AcEnemy 2를 컨트롤러에 연결
        [f]. 플레이어도 리지드바디 FreezeRotation z 축 체크
        [g]. 플레이어의 리지드바디 Mass를 5로 지정한다.

    #2. 플레이어 추적 로직
        [a]. Enemy 스크립트를 만들고 부착한다.
        [b]. 이동을 위한 속도를 속성으로 갖는다. speed;
            따라갈 목표의 리지드바디를 속성으로 갖는다. public Rigidbody2D target;
            현재 몬스터가 살아있는지 죽어 있는지 판별할 플래그를 속성으로 갖는다. bool isLive;
            본인이 물리적인 움직임을 해야 하므로 리지드바디 속성이 필요하다.
            x축을 뒤집기 위해 스프라이트 렌더러도 속성으로 필요하다. SpriteRenderer spriter;
        [c]. 리지드바디와 스프라이트렌더러를 초기화 한다.
        [d]. 물리적인 이동을 위해 FixedUpdate() 함수를 만든다.
            몬스터가 플레이어를 향해 이동하도록 할 예정이다.
        [e]. 방향을 변수로 저장한다.
            Vector2 dirVec = target.position - rigid.position;
        [f]. 앞으로 가야할 방향, 크기를 변수로 저장한다.
            Vector2 nextVec = dirVec.normalized * speed * Time.FixedDeltaTime;
        [g]. 지정된 방향으로 이동한다.
            rigid.MovePosition(rigid.position + nextVec);
        [h]. 몬스터가 로직으로 이동할 때 플레이어와의 충돌로 물리적인 움직임이 발생하지 않도록 속도를 0으로 지정한다.
            rigid.velocity = Vector2.zero;
        [i]. 씬으로 돌아가 Target 변수에 Player를 넣어 준다.
            속도는 2.5정도로 한다.
        [j]. 몬스터 이동 방향에 따라 스프라이트 x축을 반전시킬 필요가 있다.
        [k]. LateUpdate() 함수를 만든다.
            스프라이트의 x축에 값을 배정하는데 목표의 x축 값이 자신의 x축 값보다 작은지를 배정한다.
                spriter.flipX = target.position.x < rigid.position.x;
        [l]. 몬스터가 움직이기 전에 이 몬스터가 살아있는지 체크한다.
            if(!isLive) return;
                이를 반전하기 전에도 똑같이 적용한다.
        [m]. 테스트를 위해 bool 변수를 선언할 때 true를 초기화 해준다.

    #3. 몬스터 재배치
        [a]. 플레이어가 한 방향으로 계속 가면 기존에 스폰된 적들이 플레이어와 너무 멀어지게 된다.
            맵을 재배치 했었듯이 몬스터도 재배치 한다.
        [b]. Reposition 스크립트에 switch문으로 미리 Enemy 태그를 준비해 두었다.
            그 전에 몬스터가 죽었는지 살았는지를 먼저 체크한다.
                if(coll.enabled)
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
        [c]. 몬스터가 죽을 때 캡슐 콜라이더를 비활성화 할 예정이다. 
            Collider2D ( 모든 Collider2D 종류를 아우르는 클래스 ) coll; 속성을 갖는다.
                Awake() 함수에서 초기화 한다.
        [d]. 몬스터에게 Reposition 스크립트를 부착한다.
        [e]. 몬스터에게 Enemy 태그를 부착한다.
*/

/*
7. 뱀서라이크 - 오브젝트 풀링으로 소환하기

    #1. 프리팹 만들기
        [a]. 플레이어 주변에 몬스터가 무한하게 나타나도록 몬스터를 프리팹화 한다.
        [b]. 스켈레톤은 EnemyA, 좀비는 EnemyB로 명명
        [c]. 프리팹을 담을 폴더를 만든다.
            객체를 폴더로 드래그드랍한다.
                프리팹의 위치를 초기화 한다.
        [d]. 프리팹 스케일 조정자에 있는 체인 이미지를 통해 값의 균일화가 가능하다.

    #2. 오브젝트 풀 만들기
        [a]. 씬에 배치해 두었던 Enemy 객체를 모두 지운다.
        [b]. 오브젝트들을 관리한 PoolManager를 만든다.
            빈 오브젝트를 만들고 스크립트를 만들고 부착한다.
        [c]. 프리팹들을 보관할 변수와 프리팹으로 부터 만들어진 객체를 담을 '풀' 담당을 하는 리스트들이 속성으로 필요하다.
            public GameObect[] prefabs; List<GameObject>[] pools;
        [d]. 씬으로 돌아가서 속성에 프리팹을 담아준다.
        [e]. List를 초기화 한다.
            pools = new List<GameObject>[prefabs.Length];
            for(int index = 0; index < prefabs.Length; index++)
                pools[index] = new List<GameObject>();

    #3. 풀링 함수 작성
        [a]. 어디에서나 풀을 사용하기 위해 public 함수를 만든다.
            public GameObject Get(int index)
        [b]. 만들어진 인스턴스를 저장할 게임 오브젝트 변수를 만든다.
            GameObject select = null; reutrn select;
        [c]. 선택한 풀의 놀고 있는 게임 오브젝트에 접근
            발견하면 select 변수에 할당
                foreach(GameObject item in pools[index]) 로 리스트의 해당 배열을 순회
                    if(!item.activeSelf) 비 활성화 된 게임 오브젝트를 찾는다.
                        select = item; select.SetActive(true); break;
        [d]. 풀 안의 모든 게임 오브젝트가 다 씬에 배치된 상태라면 새롭게 인스턴스를 생성하여 select에 배정
            if(!select)
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select); 만들어진 객체는 리스트에 저장한다.
                select.SetActive(true);

    #4. 풀링 사용해 보기
        [a]. 만들어둔 Get() 함수를 사용하기 위해 Player의 자식으로 빈 오브젝트를 만든다.
            Spawner로 명명하고 스크립트를 만들고 부착 Spawner
        [b]. PoolManager를 여러 클래스에서 사용할 예정이므로 GameManager가 속성으로 갖도록 한다.
            public PoolManager pool;
            씬으로 돌아가서 풀 매니저를 담아준다.
        [c]. 이제 Spawner 클래스에서 게임 매니저 instance를 받아와서 사용하면 된다.
        [d]. Update() 함수에서 소환하도록 한다.
            테스트를 위해 Jump버튼이 눌렸을 때 몬스터를 소환하도록 한다.
                if(Input.GetButtonDown("Jump"))
                    GameManager.instance.pool.Get(Random.Range(0, 2));
        [e]. 이렇게 생성을 해보면 에러가 발생하는데 이는 프리팹이 플레이어의 리지드바디를 속성으로 받지 못하여서 그렇다.
            Enemy가 활성화 될 때 플레이어의 리지드바디 속성을 받기로 하자.
                OnEnable() 함수를 만든다.
                게임 매니저의 instance를 통해서 리지드바디 속성을 받아온다.
                    target = GameManager.instance.player.GetComponent<Rigidbody2D>();

    #5. 주변에 생성하기
        [a]. 이제 점프 키를 통한 소환이 아닌 재대로된 소환을 해보자.
        [b]. Player객체의 자식인 Spawner에게 다시 자식으로 빈 오브젝트를 만든다.
            Point로 명명
            Point의 좌표를 플레이어 카메라 밖으로 위치시켜서 여러 개로 복사한다.
            그러면 Player의 주위에 스폰 포인트가 만들어 진다.
        [c]. Spawner 클래스가 자신의 자식의 위치를 랜덤하게 받아와서 적을 소환하도록 한다.
            public Transform[] spawnPoint;
        [d]. 소환 타이머를 위한 변수도 속성으로 받는다.
            float timer;
        [e]. Transform[]을 Awake()에서 초기화 하도록 한다.
            spawnPoint = GetComponentsInChildren<Transform>();
        [f]. Update() 함수에서 소환 타이머를 계속 증가 시킨다.
            timer += Time.deltaTime;
        [g]. 만약 0.2초가 되었다면 소환한다.
            if(timer > 0.2f)
                그리고 timer는 초기화 한다.
        [h]. 소환을 진행할 함수를 만든다.
            void Spawn()
                GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        [i]. 받은 몬스터 객체의 위치를 스폰 포인트로 지정한다.
            마찬가지로 랜덤하게 위치를 지정하는데 현재 Transform[]에 0은 자기 자신의 위치가 저장되어 있으므로 1부터 시작한다.
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
*/

/*
8. 뱀서라이크 - 소환 레벨 적용하기

    #1. 시간에 따른 난이도
        [a]. 게임 매니저가 컨트롤 하기 위해 게임 시간과 최대 게임 시간을 속성으로 갖는다.
            public float gameTime; public float maxGameTime = 2 * 10f;
        [b]. Update() 함수에서 시간을 계속해서 증가 시킨다.
            gameTime += Time.deltaTime;
        [c]. 게임 시간이 최대 게임 시간보다 커질 경우 게임 시간에 최대 게임 시간을 배정한다.
        [d]. 이 게임 시간을 Spawner 클래스에서 활용하도록 한다.
            레벨을 담당할 변수를 속성으로 갖는다.
                int level;
        [e]. Update() 함수에서 레벨에 게임 매니저에서 게임 시간을 10으로 나눈 값을 배정한다.
            level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);
            기존에는 timer가 0.2초보다 클 때 소환을 하였는데 이제는 레벨을 이용하여 소환 타임을 지정하도록 한다.
                if(timer > (level == 0 ? 0.5f : 0.2f))
        [f]. Spawn() 함수로 가서 레벨에 따라 몬스터 인스턴스를 다르게 만든다.
            GameObject enemy = GameManager.instance.pool.Get(level);

    #2. 소환 데이터
        [a]. Spawner 스크립트에서 소환 데이터를 저장한다.
        [b]. Spawner 스크립트에 새로운 클래스를 만든다.
            public class SpawnData
        [c]. 속성으로 몬스터 타입( 정수형 )과 소환 시간, 몬스터마다의 체력, 몬스터 이동 속도
            public int spriteType; public float spawnTime; public int health; public float speed;
        [d]. Spawner 클래스 안에 만든 클래스를 속성으로 받는다. 이떄 레벨마다 각각의 소환 데이터가 필요하기 때문에 배열로 만든다.
            public SpawnData[] spawnData;
        [e]. 직렬화 : 개체를 저장 혹은 전송하기 위해 변환
            SpawnData 클래스 윗 줄에 [System.Serializable]
        [f]. 씬으로 돌아가면 Spawner 객체에 SpawnData 클래스 정보가 출력되어 있다.
            배열 크기를 2개로 만들고 속성에 값을 배정한다.
            0 0.7 10 1.5 | 1 0.2 15 2.4

    #3. 몬스터 다듬기
        [a]. 프리팹 폴더로 가서 스켈레톤 프리팹을 제거한다.
        [b]. EnemyB의 이름을 Enemy로 명명
            Speed를 0으로 지정
        [c]. Enemy 스크립트에서 스폰 데이터를 받을 수 있는 구조로 수정한다.
            애니메이터를 받을 수 있는 속성 : RunTimeAnimatorController[]를 public으로 받는다.
                animCon;
            체력 : public float health; public float maxHealth;
        [d]. 씬으로 나가서 애니메이션 컨트롤러 변수에 2 배정
            애니매이터 컨트롤러 1, 2를 배정
        [e]. void OnEnable() 함수로 돌아가서 이 몬스터가 살아있는지 죽어 있는지 작성한다.
            isLive = true; health = maxHealth;
        [f]. SpawnData를 받기 위한 함수를 만든다.
            public void Init(SpawnData data)
        [g]. 전달받은 매개변수의 속성을 현재 활성화된 몬스터 속성으로 배정한다.
            anim.runtimeAnimatorController = animCon[data.spriteType];
                Type은 몬스터의 애니메이션 인덱스로 사용한다. 그런데 Enemy 스크립트에 현재 없으므로 속성으로 애니메이터를 추가한다.
            speed = data.speed; maxHealth = data.health; health = data.health;

    #4. 소환 적용하기
        [a]. 풀매니저 배열을 1개로 수정한다.
        [b]. Spawner 스크립트로 가서 Spawn함수의 인스턴스화를 0으로 바꾼다.
            GameObejct enemy = GameManager.instance.pool.Get(0);
        [c]. SpawnData에서 만들어둔 spawnTime 속성을 가지고 소환하는 시간을 지정할 것이다.
            레벨을 SpawnData배열의 인덱스로 활용하면 레벨에 따른 각기 다른 소환 시간을 지정할 수 있다.
                if(timer > spawnData[level].spawnTime)
        [d]. Spawn()함수에서 인스턴스를 할 때 적의 스크립트에 접근하여 SpawnData를 초기화 해준다.
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        [e]. 이대로 실행할 경우 level이 spawnData배열을 넘는 값이 채워지면서 에러가 발생하므로 level값을 배정하는 구문에 수정이 필요하다.
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
*/

/*
9. 뱀서라이크 - 회전하는 근접무기 구현

    #1. 프리팹 만들기
        [a]. 스프라이트 폴더에 가면 Props 아틀라스를 찾을 수 있다.
            샆 모양의 스프라이트를 플레이어 머리위에 위치 시켜보자.
        [b]. Bullet 이라는 태그를 추가하고 샆에 지정한다.
        [c]. Bullet 스크립트를 만들고 부착한다.
        [d]. 데미지와 관통타입을 속성으로 갖는다.
            public float damage; public int per;
        [e]. 속성을 초기화할 함수를 만든다.
            public void Init(float damage, int per)
                this.damage = damage; this.per = pre;
        [f]. 씬으로 돌아가서 샆 객체에 박스콜라이더를 추가한다.
            넉백은 없으므로 트리거 체크
        [g]. 프리팹화 한다.
            위치 초기화

    #2. 충돌 로직 작성
        [a]. Enemy가 플레이어 무기에 피격되었을 때의 반응을 작성한다.
            Enemy 스크립트로 가서 충돌 이벤트 함수를 만든다.
            OnTriggerEnter2D
        [b]. 충돌한 콜라이더의 태그가 총알일 때만 반응하도록 한다.
            if(!other.CompareTag("Bullet")) return;
        [c]. 총알과 충돌하였다면 해당 총알의 스크립트에 접근하여 데미지를 가져온다.
            health -= other.GetComponent<Bullet>().damage;
        [d]. 이제 만약 체력이 0보다 클때와 체력이 0보다 작을 때의 로직을 작성한다.
        [e]. 죽음 함수를 만든다.
            Dead();
                gameObject.SetActive(false);
        [f]. 테스트를 위해 플레이어 자식으로 샆을 넣고 데미지를 지정해 준다.

    #3. 근접 무기 배치1
        [a]. 샆 객체를 모두 삭제 한다.
        [b]. 풀 매니저 속성으로 만들어 두었던 프리팹 배열에 샆 프리팹을 등록해 준다.
        [c]. 샆을 배치할 때 기준점이 되어줄 부모 오브젝트를 만든다.
            Player의 자식으로 빈 오브젝트를 만든다.
                Weapon0으로 명명
                Weapon 스크립트를 만들고 부착한다.
        [d]. Weapon 클래스는 풀 매니저에서 받아온 무기를 관리하는 역활을 갖는다.
        [e]. 무기의 ID, 프리팹 ID, 데미지, 개수, 속도를 public 속성으로 갖는다.
            int id; int prefabId; float damage; int count; float speed;
        [f]. 초기화를 위한 함수를 만든다.
            public void Init()
                무기의 id 속성에 따라 각기 다른 초기화가 이루어지므로 switch문으로 작성한다.
                    case 0: 일때 (샆) 회전 속도를 -150으로 초기화 한다.
        [g]. 근접 무기를 배치하기 위한 함수를 만든다.
            void Batch()
                반복문을 돌면서 count수 만큼 풀 매니저에 등록한 프리팹을 가져올 예정이다.
                게임 매니저의 인스턴스화 함수를 호출한다. 이 때 매개변수로 PoolManager 속석으로 등록해 두었던 프리팹의 순서(2번째)를 전달한다.
                    Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                총알을 만들면서 동시에 해당 무기의 위치를 변수로 저장하였다.
                Transform.에는 parent 속성이 있는데 bullet의 parent로 자신의 위치를 배정하면 씬에서 만들어 두었던 Weapon 0객체의 자식으로 bullet이 등록된다.
                    bullet.parent = transform;
                총알 스크립트에 접근하여 초기화 함수를 호출하여 데미지와 관통 여부를 전달한다.
                    bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        [h]. 만든 초기화 함수는 Start() 함수에서 호출하도록 한다.
        [i]. 씬으로 돌아가서 Weapon 속성을 임시로 작성해 준다.
            0 1 11 1 0
        [j]. 샆을 z축으로 회전 시키기 위해 Update() 함수에서 switch문으로 무기 id에 따라 근접 무기일 경우 회전 시키도록 한다.
        [k]. transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        [l]. 임시로 샆 객체의 y축을 1로 지정한다.
        [m]. 샆의 오더인레이어를 3으로 지정한다.

    #4. 근접 무기 배치2
        [a]. 샆 프리팹을 Weapon 0의 자식으로 등록하고 y 축을 1.5
            복사한뒤 z축 회전을 180, 위치 y축을 -1.5f로 지정, 두 개 더 복사 하여 9시와 3시
            12시 -> 9시 -> 6시 -> 3시 순으로 배치
        [b]. Weapon 스크립트의 Batch() 함수로 가서 샆의 위치를 지정해 준다.
            회전을 위해 회전 벡터를 지역 변수로 만든다. 360도를 기준으로 갯수에 따라 각도가 정해진다.
                Vector3 rotVec = Vector3.forward * 360 * index / count;
            각도를 총알에 반영한다.
                bullet.Rotate(rotVec);
            자신의 방향 기준에서 위로 위치를 지정한다. 그리고 이동하는 방향은 World를 기준으로 잡는다.
                bullet.Translate(bullet.up * 1.5f, Space.World);
        [c]. 샆 객체를 모두 지운다.

    #5. 레벨에 따른 배치
        [a]. Weapon 스크립트로 가서 레벨에 따라 샆의 개수가 늘어나게 할 예정이다.
        [b]. 레벨업 함수를 만든다.
            public void LevelUp(float damage, int count)
            레벨업을 하면 데미지와 count가 증가하므로 매개변수를 받는다.
                this.damage = damage; this.count += count;
        [c]. 만약 id가 0일 경우에 Batch() 함수를 호출한다.
        [d]. 테스트를 위해 Update() 함수에 점프 버튼이 눌렸을 때 레벨업 함수를 호출하도록 한다.
        [e]. Warld의 위치에서 샆을 생성 하였기 때문에 Batch() 함수에서 샆을 만들기 전에 local 위치로 초기화를 먼저 진행 한다.
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
        [f]. 기존에 오브젝트 풀링에서만 가져와서 샆을 만들었다면, 이미 만들어진 샆이 있을 때는 해당 샆을 사용하기로 한다.
            자신이 가지고 있는 자식 오브젝트의 갯수를 세서 만들어야할 무기 인덱스 값을 비교한다.
                if(index < transform.childCount)
            이미 만들어 둔 자식이 있다면 자식을 활용한다.
                bullet = transform.GetChild(index);
*/

/*
10. 뱀서라이크 - 자동 원거리 공격 구현

    #1. 몬스터 검색 구현
        [a]. 멀리있는 적에게 총알을 쏘기위해 플레이어에게 접근하고 있는 몬스터를 인식하는 스케너를 만든다.
            이 스케너가 몬스터만을 인식할 수 있도록 Layer를 추가하도록 한다. Enemy
                tag는 그저 이름표에 불과하다면 Layer는 물리, 시스템 상으로 구분짓기 위한 요소
        [b]. 몬스터 프리펩에 레이어 지정
        [c]. 몬스터 스캔을 담당할 Scanner 스크립트를 만든다.
        [d]. 스케너에게 필요한 속성
            스켄을 할 범위, 어떤 오브젝트를 스캔할 것인지 LayerMask, 스캔 결과를 저장할 배열, 스캔한 몬스터 중 가장 가까운 위치에 있는 몬스터를 저장할 위치 변수
                public float scanRange; public LayerMask targetLayer; public RaycastHit2D[] targets; public Transform nearestTarget;
        [e]. FixedUpdate()에서 스캔을 진행한다.
            타겟에 CircleCastAll로 빔을 쏘고 모든 결과를 받아 온다.
                targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        [f]. 빔을 통해 플레이어 주변에 있는 몬스터 검색이 되었다면 가장 가까이에 있는 몬스터를 찾는다.
            Transform GetNearest()
        [g]. 거리를 비교하기 위하여 지역 변수를 만든다. 그 변수에는 가장 먼 거리를 배정해 놓는다.
            foreach문으로 배열을 순회하며 거리를 비교한다.
                foreach(RaycastHit2D target in targets)
                    Vector3 myPos = transform.position;
                    Vector3 targetPos = target.transform.position;
                    float curDiff = Vector3.Distance(myPos, targetPos);
                    if(currDiff < diff) diff = curDiff; result = target.transform;
        [h]. FixedUpdate()에서 비교 함수를 호출하여 가장 가까이에 있는 적을 속성에 저장한다.
            nearestTarget = GetNearest();
        [i]. 씬으로 돌아가 스케너 스크립트를 플레이어에 부착하고 스캔 범위를 지정해 준다.
            TargetLayer를 Enemy로 지정 한다.

    #2. 프리팹 만들기
        [a]. 샆을 하이어라키 창에 등록한다.
        [b]. 스프라이트를 Props 아틀라스에서 Bullet3로 교체 한다.
        [c]. 콜라이더를 리셋 시킨다.
        [d]. Bullet 1 로 명명
        [e]. 프리팹 폴더로 프리팹화
        [f]. 트리거
        [g]. 데미지 지정

    #3. 총탄 생성하기
        [a]. 근접 공격을 관리해 왔던 Weapon 0 게임 오브젝트를 복사한다.
            Weapon 1 속성을 바꿔 준다.
                1 2 3 0 0
        [b]. 풀 매니저에 총알 프리팹을 추가한다.
        [c]. Weapon 스크립트로 가서 Update() 함수에 로직을 추가한다.
            일정한 간격으로 총탄을 발사하기 위해 타이머를 속성으로 갖는다. float timer;
            timer += Time.deltaTime; 으로 계속 시간을 증가 시키다가 timer가 speed 값보다 크다면 총탄을 발사 한다.
            발사 함수를 만들고 timer를 초기화 한다.
        [d]. Fire() 함수를 만든다.
            가장 먼저 플레이어 주변이 탐지된 적이 있는지 부터 체크한다.
            적을 감지하는 Scanner 스크립트를 받기 위해 Scanner컴포넌트가 부착된 Player에게 Scanner 속성을 추가한다.
                public Scanner scanner;
                    Awake()에서 초기화 한다.
            Weapon 스크립트로 돌아가서 자신의 부모 Player 스크립트를 속성으로 받아온다.
                Player player;
                    Awake() player = GetComponentInParent<Player>();
            적을 감지 한다.
                if(!player.scanner.nearestTarget) return;
        [e]. 적이 있다면 총알을 받아온다.
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            buller.position = transform.position;
        [f]. Init() 함수로 가서 speed 값( 연사 속도 )을 0.3초로 바꿔 준다.

    #4. 총탄 발사하기
        [a]. 근접 무기는 Weapone 0 을 통해 근접 무기가 스스로 회전하였었다.
        [b]. 총알은 개별로 속도를 갖고 목표물을 향해 접근한다.
            총알 프리팹에 리지드바디를 추가한다.
                중력 0
        [c]. Bullet 스크립트로 가서 리지드바디를 속성으로 받는다.
        [d]. Init(... Vector3 dir) 함수에 가서 총알이 날아갈 방향을 매개 변수로 받는다.
            방향을 적용하는 경우를 제어문으로 설정한다.
                관통 값이 -1이 아닌 경우 속도를 방향으로 지정한다.
                if(per > -1) rigid.velocity = dir;
        [e]. 트리거 이벤트 함수를 만든다.
            OnTriggerEnter2D
                몬스터 태그가 아니거나 근접 무기 즉, 관통이 -1인 총알 일 경우 반환한다.
                    if(!other.CompareTag("Enemy") || per == -1) return;
        [f]. 몬스터 태그이면서 근접 무기가 아닐 경우 관통 값이 줄어 들고 만약 관통 값이 -1까지 내려 왔다면 총알을 비활성화 한다.
            비활성화 이전에 이 총알 인스턴를 재활용 하기 위해 속도 값을 초기화 한다.
                rigid.velocity = Vector2.zero;
        [g]. Weapon 스크립트로 가서 먼저 근접 무기를 초기화 할 때 매개 변수가 전달되지 않은 것을 zero 값으로 보내 준다.
        [h]. Fire() 함수로 가서 적을 향하는 방향 값을 저장한다.
            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;
        [i]. 목표를 향해 총알을 회전 시킨다.
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        [j]. 만들어진 총알을 초기화 시킨다.
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
*/

/*
11. 뱀서라이크 - 타격감있는 몬스터 처치 만들기

    #1. 피격 리액션
        [a]. Enemy 스크립트에 피격을 위한 로직을 추가 한다.
            이미 만들어 두었던 Trigger 이벤트 함수에서 체력이 달는 곳에 피격 로직을 추가한다.
        [b]. 애니매이션에 Hit 파라미터를 전달한다.
            anim.SetTrigger("Hit");
        [c]. Health가 차감될 때 넉백 효과를 주도록 한다.
            넉백 코루틴 함수를 만든다.
                IEnumerator KnonkBack()
                하나의 물리 프레임을 딜레이를 주기 위해 WaitForFixedUpdate 타입의 속성을 만든다.
                    WaitForFixedUpdate wait;
                        초기화를 시킨다.
        [d]. 코루틴에 다음 하나의 물리 프레임까지 기다리는 딜레이를 추가한다.
            yield return wait;
            플레이어 반대 방향으로 넉백을 시키기 위해 플레이어 위치와 나의 위치로 방향을 구한다.
                Vector3 playerPos = GameManager.instance.player.transform.position;
                Vector3 dirVec = transform.position - playerPos;
            구해진 방향으로 즉발적인 힘을 준다.
                rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        [e]. 현재 FixedUpdate()에서 물리적으로 몬스터가 이동하고 있다. 이로 인해 넉백이 발생하지 않고 있다.
            몬스터가 Hit 상태일 때는 몬스터가 플레이어를 향해 가는 로직을 실행시키지 않도록 제어문을 추가한다.
                if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))

    #2. 사망 리액션
        [a]. isLive 라고 만들 상태를 false로 바꾸고 불필요한 충돌을 무시하기 위해 Collider2D 속성을 만든다.
            Collider2D coll;
            콜라이더와 물리를 비활성화 한다.
                coll.enabled = false;
                rigid.simulated = false;
        [b]. 애니메이션 파라미터를 죽음을 전달한다.
            anim.SetBool("Dead", true);
        [c]. 죽을 때 다른 몬스터를 가리지 않도록 스프라이트 순서를 내린다.
            sprite.sortingOrder = 1;
        [d]. 죽은 몬스터를 재활용 하기 위한 초기화 작업을 진행한다.
            만들어 두었던 활성화 함수 OnEnable()에서 비활성화 하였던 물리 및 기타 등등을 초기화 한다.
                coll.enabled = true;
                rigid.simulated = true;
                sprite.sortingOrder = 2;
                anim.SetBool("Dead", false);
                health = maxHealth;
        [e]. 죽음 애니메이션을 일정 시간 출력한 뒤에 몬스터 오브젝트를 비활성화 한다.
            Trigger에서 Dead()함수를 호출하지 말고 애니메이션에서 호출하도록 한다.
        [f]. 씬에서 DeadEnemy 1 애니메이션 이벤트를 추가한다.
            애니메이션 키프레임을 복사하여 1초에 붙여넣는다.
                Add event로 이벤트를 추가한다.
            DeadEnemy 2도 똑같이 추가한다.
        [g]. 몬스터 프리팹에서 DeadEnemy 1 애니메이션으로 들어간다.
            여기서 이벤트를 선택하면 Function에서 Dead() 함수를 호출할 수 있다.
            컨트롤러를 2 로 바꾼뒤 마찬가지로 Dead() 함수를 연결한다.

    #3. 처치 데이터 얻기
        [a]. 게임 매니저로 부터 몬스터의 경험치, 횟수 등을 받는다.
            속성으로 킬수, 레벨, 경험치를 만든다.
                public int level; public int kill; public int exp; public int[] nextExp = {10, 30, 60, 100, 150, 210, 280, 360, 450, 600};
        [b]. 게임 매니저의 속성을 헤더로 정리한다.
            Game Control, Player Info, Game Object
        [c]. 경험치 획득 함수를 만든다.
            public void GetExp()
                경험치를 받았으므로 exp++;
                그리고 불어난 경험치와 nextExp[level]의 값을 비교하여 같다면 레벨을 증가 시키고 exp를 초기화 한다.
                    level++; exp = 0;
        [d]. 킬수와 경험치가 갱신되는 때는 몬스터가 사망했을 때 이다. 
            Enemy 스크립트에서 체력이 0 밑으로 떨어 졌을 때 경험치 획득 함수를 호출한다.
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
        [e]. 반복되는 사망 로직 실행을 방지하기 위해 Trigger 초입에 몬스터가 죽었는지 확인하는 제어문을 추가한다.
*/

/*
12. 뱀서라이크 - HUD 제작하기

    #1. UI 캔버스
        [a]. 도화지, UI를 배치할 캔버스를 만든다.
            Canvas와 Canvas Scaler를 설정한다.
            UI Scale Mode를 Scale With Screen Size로 바꿔 준다.
            135 / 270
            Match는 0.5, Pixel Per unit은 18
        [b]. 캔버스에 Text UI를 추가한다.
            Size : 400 / 100, Color : 흰색, 폰트 사이즈 : 30

    #2. 스크립트 준비
        [a]. UI를 컨트롤 할 HUD스크립트를 만든다.
        [b]. 총 5가지의 정보를 다룰 예정이다.
        [c]. 열거형 속성을 추가한다.
            public enum InfoType { Exp, Level, Kill, Time, Health }
            public InfoType type;
        [d]. UI를 받을 텍스트와 슬라이트 속성을 추가한다.
            네임 스페이스를 먼저 추가 한다.
                using UnityEngine.UI;
            Text myText; Slider mySlider;
        [e]. 텍스트와 슬라이더를 초기화 한다.
        [f]. 정보를 갱신하기 위해 LateUpdate()함수를 만든다.
            switch문으로 정보의 타입에 따라 각기 다른 갱신 로직을 실행 한다.

    #3. 경험치 게이지
        [a]. 캔버스 안에 UI 슬라이더를 만든다.
            앵커를 중앙 상단으로 배치 시키고 가득 채운다.
                높이 여백을 7로 지정
        [b]. Slider의 interactable을 해제하여 플레이어가 임의로 게이지를 조정하지 못하게 한다.
            Transition None
            Navigation None
            Handle Rect를 지운다. None
            value 0.5
        [c]. Slider의 자식 오브젝트로 있는 Handle도 지운다.
        [d]. Background의 앵커를 화면을 모두 채우도록 채운다.
        [e]. FillArea도 마찬가지로 모두 채우도록 한다.
        [f]. Fill의 Left, right를 0으로
        [g]. Background의 스프라이트를 UI 아틀라스에서 Back 0 로 바꿔 준다.
        [h]. Fill의 스프라이트를 UI 아틀라스의 Front 0으로 바꿔 준다.
        [i]. 게임 매니저가 보유한 경험치에 따라 Slider의 이미지가 바뀌겠끔 HUD 스크립트로 가서 LateUpdate()함수의 Exp Case에 로직을 추가한다.
        [j]. 현재 경험치 / 최대 경험치
            float curExp = GameManager.instance.exp;
            float maxExp = GameManager.instance.nextExp[GameManager.instance.levle];
            mySlider.value = curExp / maxExp;
        [k]. Slider 오브젝트에 스크립트를 부착한다.
        [l]. Type을 Exp로 지정한다.

    #4. 레벨 및 킬수 텍스트
        [a]. Slider 오브젝트를 Exp로 명명
        [b]. 캔버스에 Text UI를 추가 한다.
            앵커를 오른쪽 위로 지정한다.
            텍스트를 오른쪽 정렬한다.
            x축 여백을 -2, y축 여백을 -8
            폰트를 neodgm으로 변경 size 6, Lv.999
            Level로 명명
        [c]. 캔버스에 Image UI를 추가한다.
            이미지 스프라이트를 UI 아틀라스에서 Icon 0으로 교체
            SetNativeSize, 앵커를 왼쪽 위로 지정한다.
            여백 2, -8
            Level 오브젝트를 복사하여 이미지 자식으로 둔다.
            자식의 앵커를 왼쪽으로 붙인다.
            자식읜 높이 사이즈를 8로 맞추고 x 여백을 9, 중앙 가운데 정렬, 9999
            자식을 Kill Text로 명명
            Kill로 명명
        [d]. HUD 스크립트로 가서 레벨과 킬수에 대한 로직을 추가한다.
        [e]. Level은 text 속성에 게임 매니저의 레벨 값을 스트링으로 변환하여 넣어 준다.
            myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
        [f]. Kill도 마찬가지
            myText.text = string.Format("{0:F0}", GameManager.instance.kill);
        [g]. Kill Text에 HUD 스크립트를 부착한다.
            타입은 Kill
        [h]. Level에도 HUD 스크립트 부착

    #5. 타이머 텍스트
        [a]. 생존 시간을 표시할 Text를 만든다.
            Level을 복사한다.
            앵커를 정 중앙 상단으로 위치 시킨다.
            Time으로 명명, 00:00
            사이즈 9, 색을 흰색, HUD를 부착하고 타입은 Time으로
        [b]. HUD 스크립트에 Time 로직을 추가한다.
        [c]. 흐르는 시간이 아닌 남은 시간을 먼저 구한다.
            float remainTime = GameManager.instance.maxGameTime - GameMnager.instance.gameTime;
        [d]. 구한 시간을 분과 초로 분리한다.
            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
        [e]. 구한 분 초를 텍스트로 출력한다.
            myText.text = string.Format("{0:D2}:{1:D2}", min, sec);

    #6. 체력 게이지
        [a]. 게임 매니저 스크립트로 가서 체력과 최대 체력 속성을 만든다.
            public int health; public int maxHealth = 100;
        [b]. 캔버스에 빈 오브젝트를 만든다.
            Health로 명명, 사이즈 12 / 4
        [c]. Exp 를 복사한뒤 Health의 자식으로 둔다.
            Health Slider로 명명
            앵커를 아래쪽 꽉채운다. 높이 4
        [d]. Background 스프라이트를 Back 1로 바꿔주고
            Fill 스프라이트를 Front 1로 바꿔준다.
            Health Slider의 위치를 높이 -12로 지정
        [e]. HUD 스크립트로 가서 Health 로직을 추가한다.
        [f]. 경험치 로직과 비슷하다.
            float curHealth = GameManager.instance.health;
            float maxHealth = GameManager.instance.maxHealth;
            mySlider.value = curHealth / maxHealth;
        [g]. Health Slider에 HUD 스크립트를 부착한다.
        [h]. 게임 매니저 스크립트에서 Start() 함수를 만든다.
            현재 체력을 초기화 해준다.
            health = maxHealth;
        [i]. 체력바가 플레이어를 따라가도록 Follow 스크립트를 만든다.
            RectTransform 속성을 갖는다.
            RectTransform rect;
                Awake()에서 초기화 한다.
            FixedUpdate()에서 플레이어를 따라다닌다.
        [j]. 월드와 스크린 좌표계가 다르므로 카메라를 가져와서 월드의 좌표계를 스크린 좌표계로 변환해 준다.
            rect.position = Camera.main.WorldToScreenPoint(GamaManager.instance.player.transform.position);
*/

/*
13. 뱀서라이크 - 능력 업그레이드 구현

    #1. 아이템 데이터 만들기
        [a]. 아이템 데이터의 생성을 담당할 스크립트를 만든다. ItemData
        [b]. 모노비해비얼을 ScriptableObject로 바꿔준다.
            이 스크립트는 근접, 원거리 공격, 전체 능력치 업 아이템, 힐링 등을 모두 포함한다.
        [c]. 헤더로 구분지으며 속성을 만든다.
        [d]. Main Info : 아이템 아이디, 이름, 설명, 아이콘, 아이템 타입
            public int itemId; public string itemName; public string itemDesc; public Sprite itemIcon; public enum ItemType { Melee, Range, Glove, Shoe, Heal } public ItemType itemType;
        [e]. Level Data : 기본 데미지, 기본 근접 무기의 갯수 or 원거리 무기의 관통 성능, 레벨 별 데미지, 레벨 별 갯수 or 관통
            public float baseDamage; public int baseCount; public float[] damges; public int[] counts;
        [f]. Weapon : 계속 쏘는 투사체의 프리팹
            public GameObject projectile;
        [g]. public class ItemData : ... 위에 
            [CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
        [h]. UndeadSurvive 폴더에 새 폴더를 생성한다. Data
            Scriptble Object 생성 Item 0 
            샆을 만든다.
                Melee, 0, 샆, 공백, Select 0, 4.5, 3, 5{0.5,1,1.5,2,3}, 5{1,1,1,1,2}, Bullet 0
        [i]. Item 0 을 복사하여 1로
            Range, 1, 엽총, 공백, Select 3, 3, 0, 5{0.35,0.7,1,1.4,2}, 5{1,1,2,3,4}, Bullet 1
        [j]. 또 복사하여 2로
            Glove, 2, 테크니컬 장갑, 공백, Select 6, 0, 0, 5{0.1,0.2,0.35,0.5,0.75}, 5{0}, None
        [k]. 또 복사하여 3으로
            Shoe, 3, 전투 장화, 공백, Select 7, 0, 0, 5{0.1,0.2,0.3,0.4,0.5}, 5{0}, None
        [l]. 또 복사하여 4로
            Heal, 4, 음료수, 공백, Select 8, 0...
        
    #2. 레벨업 버튼 제작
        [a]. 캔버스 안에 빈 오브젝트를 만든다. LevelUp
            size : 22 / 150, 앵커 오른쪽 하단
        [b]. LevelUp의 자식으로 버튼을 만든다. Item 0
            size : 22 / 30, 스프라이트를 Panel로 지정, 버튼의 텍스트를 Text Level로 명명, 텍스트 앵커를 가운데로
                size : 0 / 0, overflow, 폰트 지정, 크기 5, Lv.00, y축 -9
            Item 0의 자식으로 아이템 아이콘을 보여줄 이미지 추가
                스프라이트를 Select 0 으로 지정, SetNativeSize, y축 3, Icon으로 명명
        [c]. 업그레이드 버튼을 담당할 스크립트 생성 Item
        [d]. 아이템 관리에 필요한 속성을 만든다.
            아이템 데이터 클래스, 아이템 레벨, Weapon 클래스, 아이콘 이미지와 아이콘 텍스트
            public ItemData data; public int level; public Weapon weapon; Image icon; Text textLevel;
        [e]. Awake()에서 icon, textLevel을 초기화
            자식이 가지고 있는 컴포넌트 이므로 GetComponentsInChildren으로 가져온다.
            icon = GetComponentsInChildren<Image>()[1]; 자기 자신이 아닌 두 번째 이미지를 가져온다.
            icon.sprite = data.itemIcon; 아이템 데이터에 저장해 둔 아이콘 이미지를 배정한다.
            Text[] texts = GetComponentsInChildren<Text>();
            textLevel = text[0];
        [f]. LateUpdate()에 레벨 텍스트를 갱신하도록 한다.
            textLevel.text = "Lv." + (level + 1);
        [g]. LevelUp의 자식 Item 0에게 Item 스크립트 부착
            Item 0을 Data에 삽입
        [h]. Item 0 을 복사하여 Item 1로 명명하고 Item 1을 Data에 삽입
        [i]. LevelUp 객체에 Vertical Layout Group 컴포넌트를 부착한다.
        [j]. Item 0, 1, 2, 3, 4까지 만들고 스크립트블 오브젝트를 각각 연결한다.
            Item 스크립트도 각각 부착한다.
        [k]. Item 스크립트로 가서 임시로 레벨업 하는 로직을 추가한다.
        [l]. 버튼 클릭 이벤트와 연결할 함수를 추가한다.
            public void OnClick()
            업데이트할 아이템 타입에 따라 각기 다른 로직을 실행하기 위해 switch문을 사용한다.
                switch(data.itemType)
        [m]. Melee와 Range는 동일한 로직을 사용하므로 case를 붙여준다.
        [n]. switch문을 나오고 나면 레벨을 증가시키고 레벨이 최대일 경우 버튼의 Interactable을 비활성화
            level++; if(level == data.damages.Length)
            버튼 컴포넌트를 받아와서 비활성화 한다.
                GetComponent<Button>().interactable = false;
        [o]. Item 0, 1, 2, 3, 4에 각각 자신의 OnClick 함수를 연결 한다.
            Navigation None

    #3. 무기 업그레이드
        [a]. Player의 자식인 Weapon 0, 1을 제거한다.
        [b]. Item 스크립트에서 무기를 생성하는 로직을 추가한다.
            OnClick으로 처음 눌릴 때 (레벨에 0일 때) 게임 오브젝트를 새롭게 만든다.
                GameObject newWeapon = new GameObject();
            빈 게임 오브젝트에 컴포넌트로 Weapon을 추가해 주고 속성에 저장해 준다.
                weapon = newWeapon.AddComponent<Weapon>();
            weapon으로 Weapon 스크립트의 초기화 함수를 호출한다.
        [c]. Weapon스크립트의 Init()함수에 스크립트블 오브젝트를 매개변수로 받아 활용하도록 한다.
            public void Init(ItemData data)
            먼저 이름을 바꿔준다.
                name = "Weapon " + data.itemId;
            Player의 자식으로 지정한다.
                trnasform.parent = player.transform;
            위치를 지정해 준다.
                transform.localPosition = Vector3.zero;
            아이템의 아이디, 데미지, 카운트를 세팅한다.
                id = data.itemId; damage = data.baseDamage; count = data.baseCount;
            반복문으로 게임 매니저의 프리펩 배열을 순회하며 전달받은 데이터의 프리팹과 게임 매니저의 프리팹이 같을 때까지 반복한다. 그리고 같은 프리팹의 인덱스를 프리팹 아이디로 저장한다.
                for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
                    if(data.projectile == GameManager.instance.pool.prefabs[index])
                        prefabId = index; break;
        [d]. Weapon스크립트에서 임시로 Start()함수에서 Init()함수를 호출 했었는데 이제 삭제한다.
        [e]. Awake()에서 Player를 초기화 할 때 GetComponentInParent로 초기화 했었는데, 이제 Weapon은 시작할 떄부터 Player의 자식이 아니다.
            게임 매니저로부터 받아와 초기화 한다.
                player = GameManager.instance.player;
        [f]. Item 스크립트에서 이제 레벨업을 하였을 때 Init()에서 데미지와 카운트를 올려주도록 한다.
            다음 데미지와 다음 카운트를 변수로 저장한다.
            데미지는 기존 값에 더해 준다.
                nextDamage += data.baseDamage * data.damages[level];
            카운트는 증가값 만을 더한다.
                nextCount += data.counts[level];
        [g]. 증가한 값을 Weapon스크립트의 LevelUp함수에 전달한다.
            weapon.LevelUp(nextDamage, nextCount);

    #4. 장비 업그레이드
        [a]. 무기를 Weapon 스크립트로 관리 했듯이 유틸 아이템도 스크립트를 통해 관리하도록 한다.
            Gear
        [b]. Weapon과 비슷한 구조로 만든다.
            장비의 타입과 수치를 저장할 변수를 속성으로 만든다.
                public ItemData.ItemType type; public float rate;
        [c]. Init(ItemData data) 초기화 함수를 만든다.
            name = "Gear " + data.itemId;
            transform.parent = GameManger.instance.player.transform;
            transform.localPosition = Vector3.zero;
            type = data.itemType;
            rate = data.damages[0];
        [d]. LevelUp(float rate) 함수를 만든다.
            this.rate = rate;
        [e]. 공격 속도를 올려주는 함수를 만든다.
            void RateUp()
            플레이가 갖고 있는 모든 무기의 공격 속도를 높이기 위해 우선 플레이어가 보유한 무기 스크립트를 배열로 받는다.
                Weapon[] weapons = transform.parent.GetComponentsIn<Children<Weapon>();
            반복문으로 무기들을 순회하면 무기 타입에 따라 공격 속도값을 다르게 증가 시킨다.
                foreach(Weapon weapon in weapons)
                    switch(weapon.id)
        [f]. 신발의 기능인 이동 속도 증가 함수를 만든다.
            void SpeedUp()
            먼저 지역 변수를 만들어서 기본 이동 속도를 저장한다.
                float speed = 3;
            플레이어의 스피드에 기본 스피드 + rate 값
                GameManager.instance.player.speed = speed + speed * rate;
        [g]. 무기의 타입에 따라 함수를 호출해 주는 함수를 만든다.
            void ApplyGear()
            switch 문으로 아이템 타입에 따라 장갑은 RateUp(), 신발이면 SpeedUp() 함수를 호출한다.
        [h]. Init() 함수를 통해 Gear가 초기화 될 때 ApplyGear를 호출하도록 한다.
            그리고 레벨업을 했을 때 속도rate를 증가 시키면서 ApplyGear 함수를 호출한다.
        [i]. Item 스크립트로 가서 Gear 변수를 속성으로 만든다.
            public Gear gear;
            이제 다시 OnClick 함수로 가서 Glove, Shoe 레벨업 버튼이 눌렸을 때의 로직을 작성한다.
        [j]. 레벨이 0일 때와 아닐때 제어문을 만든다.
            "무기가 빈 오브젝트를 만들고 스크립트를 넣고 Init함수를 호출했던 것"을 Gear 타입으로 그대로 따라 작성한다.
                GameObject newGear = new GameObject();
                gear = newGear.AddComponent<Gear>();
                gear.Init(data);
        [k]. 레벨이 증가하였을 경우도 다음 속도를 저장하고 Gear의 레벨업에 전달한다.
            float nextRate = data.damages[levle];
            gear.LevelUp(nextRate);
        [l]. BroadcastMessage : 특정 함수 호출을 모든 자식에게 방송하는 함수
            Weapon 스크립트에서 자신이 탄생(초기화)할 때 플레이어 자식들이 가지고 있는 모든 ApplyGear 함수를 호출한다.
            초기화 뿐만 아니라 레벨업을 할 때도 마찬가지로 호출한다.
        [m]. SendMessageOptions.DontRequireReceiver로 BroadcastMessage를 전달할 때 매개 변수로 추가한다.
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        [n]. Heal 아이템은 일회성 아이템으로 Item 스크립트에서 바로 적용한다.
            버튼이 눌린 아이템이 Heal일 경우에 게임 매니저에서 health를 maxHealth로 배정해 준다.
        [o]. Heal case에서는 레벨업이 진행되지 않도록 나머지 타입에서만 break이전에 레벨업을 하도록 한다.
*/

/*
14. 뱀서라이크 - 플레이어 무기 장착 표현하기

    #1. 양손 배치
        [a]. 플레이어의 자식으로 스프라이트를 만든다. Hand Left
            프롭스 아틀라스에서 Weapon 0을 스프라이트로 지정한다.
            OrderInLayer를 6으로 지정
            위치를 손 위치로 바꿔준다. rotation도 z축으로 약간 수정해 준다. -35
        [b]. 복사하여 Hand Right으로 만든다.
            위치를 오른손으로 지정하고 스프라이트는 Weapon3 으로 지정한다.
            OrderInLayer를 4로 지정

    #2. 반전 컨트롤 구현
        [a]. Hand 스크립트를 만든다.
        [b]. 현재 손이 오른손인지 왼손인지 구분할 변수를 속성으로 갖는다.
            public bool isLeft;
        [c]. 스프라이트 렌더러, 플레이어의 스프라이트 렌더러를 속성으로 갖는다.
        [d]. Awake() 함수에서 플레이어 스프라이트를 초기화 한다.
            player.GetComponentsInParent<SpriteRenderer>()[1];
        [e]. 왼손은 방향, 오른손은 위치에 대한 정보를 속성으로 저장한다.
            Vector3 rightPos = new Vector3(0,34f, -0.15f, 0);
            Vector3 rightPosReverse = new Vector3(-0.15f, -0.15, 0);
            Quaternion leftRot = Quaternion.Euler(0, 0, -35);
            Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
        [f]. 씬으로 나가서 왼손 오른손에 각각 스크립트를 부착한다.
            public Sprite에 직접 스프라이트 렌더러를 넣어 준다.
            왼손의 경우 isLeft를 체크한다.
        [g]. LateUpdate()를 만들고 플레이어의 상태를 불값으로 저장한다.
            bool isReverse = player.flipX;
        [h]. 현재 손이 왼손인지 오른손인지 구분하여 로직을 실행한다.
            근접무기(왼손)의 경우 플레이어를 기준으로 회전을 시킨다.
                transform.localRotation = isReverse ? leftRotReverse : leftRot;
            근접무기의 스프라이트 반전을 실행한다. 레이어 순서를 바꾼다.
                spriter.flipY = isReverse;
                spriter.sortingOrder = isReverse ? 4 : 6;
            원거리무기(오른손)의 경우 플레이어를 기준으로 위치를 잡는다.
                transform.localPosition = isReverse ? rightPosRevers : rightPos;
            근접무기의 스프라이트 반전을 실행한다. 레이어 순서를 바꾼다.
            원거리무기의 스프라이트 반전을 실행한다. 
                spriter.flipX = isReverse;
                spriter.sortingOrder = isReverse ? 6 : 4;

    #3. 데이터 추가
        [a]. 기존 왼손 오른손에 지정하였던 스프라이트를 None으로 지정한다.
            그리고 손 두개를 비활성화 한다.
        [b]. ItemData 스크립트에 데이터를 추가한다.
            public Sprite hand;
        [c]. 스크립트블 오브젝트에서 아이템 0과 1이 무기 오브젝트이다.
            아이템 데이터 속성에 스프라이트를 넣어 준다.

    #4. 데이터 연동
        [a]. Player 스크립트에서 속성으로 만들어 보관하도록 한다.
            public Hand[] hands;
        [b]. Awake()에서 초기화 한다.
            hands = GetComponentsInChildren<Hand>(true);
            비활성화 시켜놓은 오브젝트는 GetComponent로 받아지지 않으므로 매개변수로 true를 전달한다.
        [c]. Weapon 스크립트로 가서 무기가 생성될 때 초기화 함수가 무조건 호출되는데 이 때 손을 출력하도록 한다.
            손 오브젝트를 가져온다.
                Hand hand = player.hands[(int)data.itemType];
            가져온 손에 스프라이트를 적용 시킨다.
                hand.spriter.sprite = data.hand;
                hand.gameObject.SetActive(true);
*/

/*
15. 뱀서라이크 - 레벨업 시스템

    #1. UI 완성하기
        [a]. 캔버스에 이미지를 만든다.
            바탕 이미지로 활용하기 위해 앵커를 가득 채우고 검은색으로 하고 알파값을 150으로 낮춘다.
            LevelUp으로 명명
            기존 LevelUpUI는 ItemGroup으로 명명
        [b]. LevelUp에 이미지를 만든다. 
            스프라이트를 Panel로 지정한다.
            100 / 130 사이즈, 색을 녹색으로 지정
            Panel로 명명
        [c]. Panel에 Text를 만든다.
            앵커는 상단을 가득 채운다.
            중앙 정렬로 하고 height는 -3, 폰트를 neodgm, 사이즈 8, 색 흰색
            Shadow 컴포넌트 부착, x 0.5, y -0.5
            축하합니다! 레벨이 상승했습니다.
            Text Title로 명명
        [d]. ItemGroup을 Panel 자식으로 옮긴다.
            앵커를 화면이 꽉차게 지정한다.
            ItemGroup의 자식인 Item 0,1,2,3,4이미지를 맞추기 위해 컴포넌트 VerticalLayoutGroup에서
                ControlChildSize : Width, Height를 체크하여 자식 오브젝트를 자신의 크기에 맞게 자동 변경 시킨다.
                Spacing을 1로 준다.
            좌우,아래 여백 5, 위 25
            Item 3, 4는 비활성화 한다.
        [e]. Item 0의 Icon의 앵커를 왼쪽을 붙인다. xy 여백을 6/3
            Text Level의 폭을 18로 지정하고 앵커를 왼쪽으로 붙인다. xy여백을 6/-10
            텍스트 레벨을 복사하여 Text Name으로 명명
                폰트 사이즈 7로 지정 텍스트 왼쪽 정렬, x여백 27, y여백 6
                아이템 이름
            텍스트 네임을 복사하여 Text Desc로 명명
                폰트 사이즈 5로 지정, y여백 -7
            텍스트를 각각 다른 색으로 지정
        [f]. Item 1, 2, 3, 4를 모두 지운다.
            Item 0 을 복사한다. Item 0, 1, 2, 3, 4
            데이터 속성에 스크립트블 오브젝트를 넣어 준다.
            Item 3, 4는 비활성화

    #2. 아이템 텍스트
        [a]. 스크립트블 오브젝트에 아이템 설명란은 모두 공백으로 두었다.
        [b]. ItemData 스크립트로 가서 itemDesc 속성을 찾는다.
            바로 윗줄에 인스펙터 창에서 텍스트를 여러 줄 넣을 수 있게 TextArea 속성을 부여한다.
                [TextArea]
        [c]. 스크립트블 오브젝트에 Item Desc 항목을 채운다.
            데미지 {0}% 증가    데미지 {0}% 증가    연사속도 {0}% 증가  이동속도 {0}% 증가  생명력 전체 회복
            회전체 {1}개 추가   관통력 {1} 증가
        [d]. 아이템 이름과 설명을 UI창에 띄운다.
            Item 스크립트에서 로직을 추가한다.
        [e]. 속성으로 아이템 이름과 설명을 만든다.
            Text textName; Text textDesc;
            Awake()에서 초기화 한다.
                GetComponents의 순서는 계층구조의 순서를 따라가므로 배열의 인덱스를 하이어라키 창에 등록된 순서로 지정하면 된다.
                textName = texts[1]; textDesc = texts[2];
                textName.text = data.itemName;
        [f]. 활성화 함수를 만든다. OnEnable()
            LateUpdate()에서 실행하던 레벨 출력 로직을 활성화 함수로 옮긴다.
            LateUpdate()는 지운다.
        [g]. 아이템 설명을 매개 변수를 통해서 데이터에 문자열로 저장할 것이다.
            textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
            이때 무기는 설명이 2줄이기 때문에 switch문으로 구분하여 각기 다른 설명을 저장한다.

    #3. 창 컨트롤
        [a]. LevelUp 오브젝트의 크기를 0 ~ 1로 변형 시켜 나타나게 할 예정
            LevelUp 스크립트를 생성한다.
        [b]. UI이므로 RectTransform을 속성으로 갖는다.
            RectTransform rect;
        [c]. Awake()함수에서 초기화 한다.
            rect = GetComponent<RectTransform>();
        [d]. 창을 띄우는 함수와 숨기는 함수를 만든다.
            public void Show() public void Hide()
        [e]. rect.localScale = Vector3.one; rect.localScale = Vector3.zero;
        [f]. 레벨업을 관리하는 게임 매니저가 Show 함수를 호출한다.
            속성으로 게임 오브젝트를 받는다.
                public LevelUp uiLevelUp;
        [g]. 씬으로 나가 LevelUp오브젝트에 스크립트를 부착한다.
            게임 매니저 속성에 LevelUp 오브젝트를 넣어 준다.
        [h]. 게임 매니저 스크립트로 가서 레벨업을 하는 구간 GetExp에서 Show 함수를 호출한다.
        [i]. Item 0, 1, 2, 3, 4의 OnClick을 추가하여 LevelUp 오브젝트를 넣는다.
            Hide 함수는 레벨업 버튼이 눌렸을 때 호출되게 한다.

    #4. 기본 무기 지급
        [a]. LevelUp 스크립트에서 근접 무기를 가져오도록 한다.
            속성으로 Item[] 배열을 받는다.
                Item[] items;
            Awake() 함수에서 초기화 한다.
                items = GetComponentsInChildren<Item>(true);
        [b]. 0번째 무기를 선택하여 불러오는 함수를 만든다.
            public void Select(int index)
            해당 아이템의 클릭을 로직으로 실행 한다.
                items[index].OnClick();
        [c]. 게임 매니저가 Start()함수에서 임시로 Select 함수를 호출한다.
            uiLevelUp.Select(0);

    #5. 시간 컨트롤
        [a]. 레벨업을 할 때 시간이 정지하도록 게임 매니저에서 시간을 관리하도록 한다.
        [b]. 시간을 멈출지 체크할 bool 속성을 만든다.
            public bool isLive;
        [c]. 시간을 컨트롤 하는 함수를 만든다.
            public void Stop() public void Resume()
        [d]. Stop에서는 isLive = false가 되고 게임에서 진행되는 시간을 멈춘다.
            Time.timeScale = 0;
        [e]. Resume() 함수는 반대로 한다.
        [f]. LevelUp 스크립트에서 레벨업을 할 떄 시간 조정 함수를 호출한다.
            Show() GameManager.instance.Stop();
            Hide() GameManager.instance.Resume();
        [g]. isLve를 사용하여 멈춰야 할 다른 객체에 모두 적용한다.
            스크립트들의 Update/FixedUpdate 함수를 모두 살피며 제어문을 만든다.
                GameManager, Player, Enemy, Spawner, Weapon
        [h]. 씬으로 나가서 게임 매니저의 isLive를 체크해 준다.

    #6. 랜덤 아이템
        [a]. LevelUp 스크립트에서 Next() 함수로 모든 아이템을 비활성화 하고 이 중 3개만 랜덤하게 활성화 한다.
            이때 만렙 아이템은 소비아이템으로 대체한다.
        [b]. 반복문으로 아이템을 순회 한다.
            foreach(Item item in items)
            순회 하며 전부 비활성화 한다.
                item.gameObject.SetActive(false);
        [c]. 중복되지 않는 정수형 배열을 만든다.
            int[] ran = new int[3];
            무한 반복을 진행 하며 랜덤한 값을 받는다.
                ran[0] = Random.Range(0, items.Length);
                ran[1] = Random.Range(0, items.Length);
                ran[2] = Random.Range(0, items.Length);
            값이 서로 다른지 확인하고 반복문을 탈출한다.
                if(ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
        [d]. 반복문을 돌며 랜덤한 아이템을 Item 스크립트로 저장하고 활성화 한다.
            for(int i = 0; i < ran.Length; i++)
                Item ranItem = items[ran[index]];
            만렙일 경우 소비아이템으로 대체하기 위해 제어문을 만든다.
                if(ranItem.level == ranItem.data.damages.Length)
                    items[Random.Range(4, items.Length)].gameObject.SetActive(true);
        [e]. 창을 띄우는 함수에서 Next()함수는 Show() 함수에서 호출한다.
        [f]. 이제 무한 레벨을 만들어야 한다.
            게임 매니저에서 레벨업을 할 때의 조건으로 최대 레벨일 경우 항상 최고 경험치를 넘겨야 다음 레벨이 되도록 한다.
                if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
            HUD 스크립트에서 경험치 바를 출력할 때에도 무한 레벨을 위해 최대 경험치를 확인하여 출력한다.
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
*/

/*
16. 뱀서라이크 - 게임 시작과 종료

    #1. 게임 시작
        [a]. 게임 매니저의 isLive 속성을 체크 해제 한다.
            로직을 통해 게임 시작을 하도록 할 예정이다.
        [b]. 캔버스에서 빈 오브젝트를 만든다. GameStart
            GameStart의 자식으로 Image를 만든다.
                Title 이미지를 UI 아틀라스에서 Title 0 을 스프라이트로 배치한다.
                scale 1.5
                Shadow 컴포넌트 추가, 알파값 조정
                y축 40
        [c]. GameStart의 자식으로 버튼을 만든다. Button Start
            사이즈 60/20, 판넬 스프라이트
            Navigation None
            게임 시작, font size 10
            OutLine 컴포넌트 부착 distance 0.6
            위치 -40
            Button에 Shadow 컴포넌트 부착
        [d]. 게임 시작 버튼이 눌리면 게임 시작 관련 UI는 숨겨져야 한다.
            Button Start의 OnClick에 GameStart를 넣는다.
                GameObject -> SetActive
        [e]. 게임 시작 전에는 HUD를 감추어 두도록 한다.
            캔버스에 빈 오브젝트를 만들고 HUD로 명명
                Exp, Level, Kill, Timer, Health를 HUD자식으로 이전
            GameStart -> Button Start의 OnClick에 HUD를 넣는다.
                GameObject -> SetActive , 체크
            HUD를 비활성화 시켜 놓는다.
        [f]. 게임 매니저에서 게임 실행 로직을 추가한다.
            기존의 Start() 함수를 GameStart() 함수로 바꾸고 public을 붙착하여 버튼에 연결한다.
            isLive = true;
        [g]. 다시 Button Start에 게임 매니저를 넣고 GameStart 함수를 호출한다.

    #2. 플레이어 피격
        [a]. 플레이어 스크립트에서 피격 함수를 만든다.
            OnCollisionStay2D
            가장 먼저 isLive 상태를 체크하여 반환한다.
        [b]. 게임 매니저의 health를 감소시킨다.
            이때 프레임마다 체력이 감소하게 한다.
                GameManager.instance.health -= Time.deltaTime * 10;
                타입을 맞추기 위해 게임 매니저의 health와 maxHealth의 타입을 float로 바꾼다.
        [c]. health가 0보다 작아지면 플레이어가 사망한다.
            플레이어가 사망하면 사망 애니메이션이 출력되면서 플레이어 객체의 자식을 비활성화 시킨다.
            반복문으로 제거할 자식 인덱스 부터 반복을 시작하여 끝까지 돌며 비활성화 한다.
                for(int index = 2; index < transform.childCount; index++)
                    transform.GetChild(index).gameObject.SetActive(false);
            비활성화 이후에 파라미터 전달
                anim.SetTrigger("Dead");

    #3. 게임 오버
        [a]. GameStart를 복사하여 GameResult로 명명
            Title을 게임 오버 Title(Title2)로 바꿔준다.
            버튼 이름을 Button Retry로 명명
            OnClick에 지정되어있던 함수들을 모두 제거한다.
            돌아가기
        [b]. 게임 매니저에서 재시작 함수를 만든다.
            public void GameRetry()
            장면을 새롭게 불러온다.
                SceneManagement 네임 스페이스를 추가
                SceneManager.LoadScene(0);
        [c]. 게임 매니저에 게임 오버 함수를 만든다.
            public void GameOver()
            코루틴 함수를 만든다. GameOverRoutine()
                isLive = false;
                yield return new WaitForSeconds(0.5f);
                uiResult.SetActive(true);
                Stop();
        [d]. 재시작 UI를 활성화 하기 위해 GameObject로 속성을 받는다.
            public GameObject uiResult;
                씬에서 바로 넣는다.
        [e]. 이제 플레이어 스크립트로 가서 애니매이션이 출력되고 게임 매니저에서 게임 오버 함수를 호출하도록 한다.
        [f]. Button Retry에 게임 매니저를 넣고 GameRetry함수를 호출한다.

    #4. 게임 승리
        [a]. 빈 오브젝트 생성 EnemyCleaner
            박스 콜라이더 추가 : size를 모든 몬스터 들이 다 들어갈 정도로 크게 1000/1000
            Bullet 스크립트 추가 : damage 1000000지정 per -1
            태그를 Bullet, 트리거
            비활성화 시켜 놓는다.
        [b]. GameResult 의 자식 이미지 Title을 Title Over로 명명하고 복사
            Title Victory
                Title 1으로 스프라이트 교체
        [c]. 게임이 종료될 때 승리하면 Title Victort를 패배하면 Title Over를 출력하도록 스크립트를 만든다.
            Result 스크립트를 GameResult에 부착한다.
        [d]. 게임 오브젝트 배열로 타이틀 2개를 받는다.
            public GameObject[] titles;
            패배, 승리 함수를 만든다.
            public void Lose() public void Win()
            패배일 때는 패배 타일틀을, 승리했을 때는 승리타이틀을 활성화 한다.
        [e]. 게임 매니저에 속성으로 두었던 uiResult의 타입을 스크립트 Result로 바꾼다.
            게임 오버 루틴에서 재시작 오브젝트를 활성화 할 때 Lose() 함수를 호출한다.
        [f]. 씬에서 게임 매니저에 Result 오브젝트를 넣는다.
            GameResult에는 패배와 승리 타이틀을 배열에 넣는다.
        [g]. 게임 매니저에서 게임 오브젝트로 EnemyCleaner를 받는다.
            public GameObject enemyCleaner
            게임 승리 함수를 만든다.
            public void GameVictory()
            GameOverRoutine을 복사하여 GameVictoryRoutine으로 활용한다.
                추가로 몬스터를 청소하는 EnemyCleaner를 활성화 시킨다.
                0.5초 쉬었다가 Result의 Win()함수를 호출한다.
        [h]. Update()함수에서 maxGameTime을 초과했을 때 승리를 호출한다.
            GameVictory();
        [i]. 재시작을 할 때 이전 코루틴 함수로 Stop함수가 호출되어 시간이 정지되어 버리는데 이 시간을 다시 활성화 하기 위해 게임 스타트 함수에서 Resume() 함수를 호출한다.
        [j]. EnemyCleaner를 통해 몬스터를 모두 죽일 때 경험치를 획득하지 못하도록 GetExp 함수에 isLive 제어문을 추가한다.
        [k]. 씬에서 타이틀 2개, 재시작 오브젝트를 비활성화 해둔다. 게임 시작 오브젝트를 활성화 한다.
*/

/*
17. 뱀서라이크 - 플레이어 캐릭터 선택

    #1. 캐릭터 선택 UI
        [a]. 캔버스의 게임 시작 오브젝트의 자식으로 빈 오브젝트를 만든다.
            Character Group 위치 y축 -40, 101/101
            Grid Layout Group 컴포넌트를 부착한다.
                50/50, 1/1
            이전에 만들어 두었던 Button Start 버튼을 Character Group의 자식으로 이전한다.
                Character 0으로 명명
                버튼의 자식으로 이미지를 만든다.
                    Stand 0 을 스프라이트로 배치
                    y축 8
                    Icon으로 명명 
                Character 0의 Text는 Text Name으로 명명
                    벼농부, 폰트 사이즈 8, 앵커를 가운데 가득 채움
                    여백 좌우 2, y축 8
                Text Name을 복사하여 Text Adv로 명명
                    y축 -16, 폰트 사이즈 5, 이동속도 10% 증가
        [b]. Character 0 을 복사하여 1로 명명
            스프라이트, 버튼 색, 이름, 설명을 바꿔준다.
                보리농부, 연사속도 10% 증가

    #2. 선택 적용하기
        [a]. 선택한 캐릭터가 0번 인덱스인지 1번 인덱스인지 게임 매니저에게 전달하기 위해 게임 매니저 스크립트로 간다.
        [b]. 캐릭터 ID를 저장할 변수를 속성으로 갖는다.
            public int playerID;
        [c]. 기존에 만들어 두었던 GameStart함수를 수정한다. 매개 변수로 id를 받는다.
            playerId = id;
            기본 무기 지급은 playerId에 따라 지급한다.
            uiLevelUp.Select(playerId % 2);
        [d]. 플레이어를 비활성화 해 두었다가 게임 시작시 나타나게 한다.
            player.gameObject.SetActive(true);
        [e]. 캐릭터 별로 다른 스프라이트, 스프라이트 애니메이션이 출력되도록 Player 스크립트에 속성으로 만들어 둔다.
            public RuntimeAnimatorController[] animCon;
                씬으로 나가서 애니메이션 컨트롤러를 배열에 집어 넣는다.
        [f]. 활성화 될 때 애니메이터에 선택된 playerId의 애니메이터를 배정하도록 한다.
            anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
        [g]. Character Group의 자식들, Character 0, 1 버튼에 OnClick을 연결 한다.
            GameManager의 GameStat() 함수를 연결하고 매개변수를 전달한다.

    #3. 캐릭터 특성 로직
        [a]. 캐릭터 특성을 관리할 스크립트 생성 Character
        [b]. 보정 값을 static으로 만든다.
            public static float Speed { get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; } }
        [c]. Player 스크립트로 가서 Character 클래스의 static 변수를 사용하도록 한다.
            OnEnable 함수에서 Player의 속도와 보정 값을 곱해 준다.
                speed *= Character.Speed;
        [d]. Gear 스크립트로 가서 SpeedUp() 함수에서도 이동 속도 보정을 해준다.
            float speed = 3 * Character.Speed;
        [e]. 다양한 캐릭터의 특수 능력을 static으로 만들어 보자
            WeaponSpeed, WeaponRate, Damage, Count
        [f]. Weapon 스크립트로 가서 무기 관련 보정 값을 추가한다.
            Init 함수에서 근접 무기의 회전 속도를 지정 했었다.
                speed = 150 * Character.WeaponSpeed;
            연사 속도
                speed = 0.5f * Character.WeaponRate;
        [g]. Gear 스크립트에서도 회전 속도와 연사 속도를 보정한다.
            float speed = 150 * Character.WeaponSpeed;
            weapon.speed = speed + (speed * rate);
            speed = 0.5f * Character.WeaponRate;
            weapon.speed = speed * (1f - rate);
        [h]. Weapon 스크립트에서 Init함수를 통해 데미지, 관통을 보정한다.
            damage = data.baseDamage * Character.Damage;
            count = data.baseCount + Character.Count;
            LevelUp 함수에서 레벨업 할 때도 보정 값을 적용 한다.
*/

/*
18. 뱀서라이크 - 캐릭터 해금 시스템

    #1. 추가 캐릭터 버튼
        [a]. 기존에 있던 Character 0, 1에서 복사를 하여 2개 더 만든다.
            Icon에서 Stand 스프라이트 변경
            감자농부 콩농부, 공격력 20% 증가, 회전, 관통 1 추가
            버튼 색 변경
            버튼의 인덱스 수정

    #2. 잠금과 해금
        [a]. Character 2를 복사하여 Character 2(Lock)로 명명
            Character 2는 비활성화 시켜 놓는다.
            Character 2(Lock)의 Button 컴포넌트를 지운다.
            이미지 색을 회색으로, Icon의 색을 검은 색으로, 이름 Text를 지운다.
            캐릭터 능력 Text를 해금 조건으로 바꾼다. 언데드 10마리 처치 시 합류
            Outline 컴포넌트도 지우고 텍스트 색을 검은 색으로
        [b]. Character 3(Lock)를 만든다.
            어느 누구나 생존 성공시 합류
        [c]. AchiveManager 로 스크립트 생성
            빈 오브젝트를 만들고 AchiveManager라 명명, 스크립트 부착
        [d]. 속성으로 잠금된 버튼과 해금된 버튼을 담을 게임 오브젝트 배열을 만든다.
            public GameObject[] lockCharacter; public GameObject[] unlockCharacter;
            씬으로 나가서 배열에 각각의 Character 2, 3, (lock)를 넣어 준다.
        [e]. 업적을 enum으로 관리한다.
            enum Achive { UnlockPotato, UnlockBean }
            Achive[] achives;
            Awake() 에서 초기화 한다.
                achives = (Achive[])Enum.GetValues(typeof(Achive));
        [f]. 저장 데이터를 초기화 할 함수를 만든다.
            void Init()
            저장 기준 값을 저장
                PlayerPrefs.SetInt("MyData", 1);
            업적 이름을 Key값으로 하여 저장해 둘 수 있는데, 더 많아 질 수 있으니
                PlayerPrefs.SetInt("UnlockPotato", 0); PlayerPrefs.SetInt("UnlockBean", 0);
            반복문으로 속성 achives를 순회 하면서 모든 업적을 초기화 한다.
                foreach(Achive achive in achives)
                    PlayerPrefs.SetInt(achive.ToString(), 0);
        [g]. Awake() 함수에서 MyData가 저장되어 있지 않을 경우 Init함수를 호출하여 초기화 한다.
            if(!PlayerPrefs.HasKey("MyData"))
        [h]. 해금 함수를 만든다. UnlockCharacter
            잠금 버튼 배열을 순회하면서 인덱스에 해당하는 업적 이름을 가져온다.
                string achiveName = achives[index].ToString();
            0으로 저장해 두었던 업적이 1이 되었는지 체크 한다.
                bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            bool 지역 변수를 통해 버튼을 활성화 한다.
                lockCharacter[index].SetActive(!isUnlock);
                unlockCharacter[index].SetActive(isUnlock);
        [i]. Edit -> Clear All PlayerPrefs로 데이터를 지우고 테스트를 진행한다.

    #3. 업적 달성 로직
        [a]. AchiveManager에서 해금 조건을 확인한다.
            LateUpdate() 함수에서 해금을 확인하도록 한다.
        [b]. 업적 달성을 위한 함수를 만든다.
            void CheckAchive(Achive achive)
        [c]. LateUpdate()에서 프레임마다 모든 업적을 확인하기 위해 반복문을 돈다.
            foreach(Achive achive in achives)
                CheckAchive(achive);
        [d]. CheckAchive에서 어느 업적이 활성화 되었는지 확인 작업을 실시 한다.
            지역 변수로 성공 실패 값을 저장할 수 있게 한다.
                bool isAchive = false; 
            switch문으로 매개변수로 전달받은 업적에 따라 다른 로직을 실행 한다.
                isAchive = GameManager.instance.kill >= 10;
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
        [e]. 지역 변수가 true를 저장하고 PlayerPrefs에  전달 받은 매개 변수가 아직 해금되지 않았다면 업적 해금을 저장한다.
            if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)

    #4. 해금 알려주기
        [a]. 캔버스에서 Image를 만든다.
            스프라이트에 판넬을 등록한다.
        [b]. 크기 : 50/27, 앵커 오른쪽 상단, 여백 -3/-20
            Notice로 명명, Shadown 컴포넌트 부착
        [c]. Notice의 자식으로 빈 오브젝트를 만든다. UnlockPotato
            UnlockPotato 자식으로 이미지를 만든다.
                감자 농부 스프라이트를 등록한다.
                앵커 왼쪽, 여백 1/1
                Icon으로 명명
            자식으로 텍스트를 만든다.
                앵커 중앙 채움, 가운데 정렬, 좌우 여백 20/1, 폰트, 감자농부가 다음 전투에 함류합니다.
        [d]. UnlockPotato를 복사하여 UnlockBean으로 명명
            스프라이트 변경
        [e]. Notice의 자식 둘을 모두 비활성화 한다. 그리고 Notice도 비활성화 한다.
        [f]. AchiveManager에게 속성이 되어 들어간다.
            public GameObject uiNotice;
        [g]. CheckAchive() 함수에서 조건을 달성하는 순간 코루틴으로 알림창을 띄웠다가 숨길 것이다.
            NoticeRoutine()
            WaitForSecondsRealtime 속성을 만든다. wait;
        [h]. Awake()에서 초기화 한다.
            wait = new WaitForSecondsRealtime(5);
        [i]. 다시 코루틴에서 속성을 활용하여 쉬도록 한다.
            uiNotice활성화
            yield return wait;
            uiNotice비활성화
        [j]. 어떤 안내문을 띄울지 CheackAchive함수에서 조건을 달성할 때 활성화 시킨다.
            반복문으로 Notice의 자식들을 순회 하면서 조건에 맞는 자식을 활성화 한다.
            bool 지역 변수를 만들어서 반복문 인덱스와 조건을 달성한 achive의 값이 같은지를 저장한다.
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObejct.SetActive(isActive);
*/

/*
19. 뱀서라이크 - 편리한 오디오 시스템 구축

    #1. 유니티의 오디오
        [a]. 오디오 클립 : 오디오 및 사운드 파일 에셋 타입
        [b]. 빈 오브젝트를 만든다. AudioManager
            클립을 드래그 드랍
        [c]. 오디오소스 : 에셋인 오디오 클립을 재생시켜주는 컴포넌트
        [d]. 오디오리스너 : 장면에서 재생 중인 오디오를 듣는 컴포넌트

    #2. 오디오 매니저
        [a]. 오디어 매니저에 부착한 오디오 소스를 지운다.
        [b]. 오디오매니저 스크립트를 만들고 부착한다.
        [c]. 어느 클래스에서든 사용할 수 있도록 정적 메모리에 담는다.
            public static AudioManager instance;
            Awake()
                instance = this;
        [d]. 속성을 구분하기 위해 헤더로 BGM을 만든다.
            [Header("#BGM")]
            배경음과 관련된 클립, 볼륨, 오디오소스를 속성으로 갖는다.
            public AudioClip bgmClip; public float bgmVolume; AudioSource bgmPlayer;
        [e]. 다음으로 효과음을 헤더로 만든다.
            [Header("SFX")]
            배경음과 일부 동일하다. 추가로 채널 시스템이 있다.
            public AudioClip[] sfxClip; public float sfxVolume; public int chennels; AudioSource[] sfxPlayer; int chennelIndex;
        [f]. 오디오를 초기화할 Init함수를 만든다.
        [g]. 배경음 플레이어 초기화, 효과음 플레이어 초기화
            게임 오브젝트를 지역 변수로 만들고 초기화 한다.
                GameObject bgmObject = new GameObject("BgmPlayer");
            배경음을 담당할 자식 오브젝트를 만든다.
                bgmObject.transform.parent = transform
            오브젝트에 오디오소스를 생성하고 변수에 저장한다.
                bgmPlayer = bgmObject.AddComponent<AudioSource>();
            캐릭터를 선택할 때부터 bgm을 출력한다.
                bgmPlayer.playOnAwake = false;
                bgmPlayer.loop = true;
                bgmPlayer.volume = bgmVolume;
                bgmPlayer.clip = bgmClip;
        [h]. 효과음
            GameObject sfxObject = new GameObject("SfxPlayer");
            sfxObject.transform.parent = transform;
            채널값을 사용하여 오디오 소스 배열 초기화
                sfxPlayer = new AudioSource[channels];
            배열의 원소를 반복문으로 초기화 한다.
                sfxPlayer[index] = sfxObject.AddCompoenet<AudioSource>();
                sfxPlayer.playOnAwake = false;
                sfxPlayer.volume = sfxVolume;
        [i]. 씬으로 나가서 속성에 값을 채워 준다.
            채널 갯수는 16개 정도로 지정한다.

    #3. 효과음 시스템
        [a]. 효과음을 sfx 클립 배열에 넣는다.
        [b]. 열거형으로 효과음을 이름으로 지정한다.
            public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }
        [c]. 효과음 재생 함수를 만든다. PlaySfx(Sfx sfx)
            준비해둔 sfxPlayers에 매개 변수로 전달받은 효과음 종류의 클립을 저장한다.
                sfxPlayers[0].clip = sfxClips[(int)sfx];
            저장된 클립을 실행한다.
        [d]. 반복문을 만들어서 현재 클립을 재생하고 있지 않는 sfxPlayers를 찾아서 위의 플레이 로직을 실행하고자 한다.
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if(sfxPlayers[loopIndex].isPlaying)
                continue;
            channelIndex = loopIndex;
            sfxPlayers[loopIndex]...
            ... break;
        [e]. 게임 매니저에서 재생 함수를 사용한다.
            GameStart()에서 게임이 시작됬을 때 버튼 클릭 효과음이 출력된다.
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            GameOver 코루틴에서 게임 오버 Lose 효과음 출력
            GameVictort 코루틴에서 Win 효과음 출력
        [f]. AchiveManager 에서 알람이 뜰 때 NoticeRoutine에서 LevelUp
        [g]. LevelUp 스크립트의 Show, Hide에서 LevelUp, Select
        [h]. Weapon 에서 Fire 로 발사할 때 Range
        [i]. Enemy 스크립트에서 맞을 때 Hit, 죽을 때 Dead
            그런데 플레이어가 승리할 때 모든 몬스터가 한꺼번에 죽게되는데 이때는 효과음을 실행하지 않는다.
                if(GameManager.instance.isLive) 일때만 효과음 실행

    #4. 배경음 시스템
        [a]. 배경 음악을 BGM 클립에 넣는다.
        [b]. 오디오 매니저에서 효과음 플레이어 함수처럼 브금 플레이어 함수를 만든다.
            public void PlayerBgm(bool isPlay)
        [c]. 제어문을 만들어 isPlay 상태이면 BGM을 플레한다. 아닐때는 BGm을 멈춘다.
            bgmPlayer.Play();
        [d]. 게임 매니저의 게임 시작 함수와 끝 함수에서 브금 플레이어 함수를 호출한다.
        [e]. 메인 카메라에 Audio High Pass Filter 컴포넌트를 부착한다.
            비활성화 시켜 놓는다.
        [f]. 오디오 매니저에 컴포넌트를 속성으로 받는다.
            AudioHighPassFilter bgmEffect; 
            Init 함수에서 카메라로 부터 컴포넌트를 받는다.
                bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        [g]. 오디오 필터를 온오프 할 함수를 만든다.
            public void EffectBgm(bool isPlay)
                bgmEffect.enabled = isPlay;
        [h]. LevelUp 스크립트에서 레벨업 창을 띄우고 숨기는데 효과음을 각각 낼 때 온오프 함수를 호출한다.
        [i]. 효과음은 이 필터에 영향을 받지 않게 위해 오디오 매니저의 효과음 초기화에 로직을 추가한다.
            sfxPlayers[index].bypassListenerEffects = true;
*/

/*
20. 뱀서라이크 - 로직 보완하기

    #1. 무한맵 재배치 보완
        [a]. Reposition 스크립트 : 플레이어의 위치에 따라 맵의 판이 플레이어의 이동 방향으로 위치가 바뀌는 로직
        [b]. 플레이어의 입력을 바탕으로 맵의 위치를 지정하다 보니 다양한 예외상황이 발생
            플레이어의 입력을 통한 위치 지정을 지운다.
        [c]. 플레이어와의 거리를 구하고 방향을 구한다.
        [d]. 거리의 차이를 통해서 맵의 위치를 바꿔준다.

    #2. 몬스터 재배치 보완
        [a]. 플레이어와 몬스터간의 거리를 찾아 낸다.
        [b]. 거리를 몬스터의 이동 거리에 전달한다.
        [c]. 이동 거리에 * 2를 하면 플레이어의 영역에서 벗어난 몬스터가 반대쪽으로 순간이동 된다.
        [d]. 약간의 랜덤 값을 구하여 거리의 차이에 더해 준다.

    #3. 투사체 멈춤 보완
        [a]. Bullet 스크립트 : 샆, 총알, 몬스터 클리너의 데미지 및 기타 공격을 담당하는 로직
        [b]. 샆의 경우 관통력이 -1로 값을 갖고 있다.
        [c]. 총알의 관통력이 몬스터를 타격할 때마다 줄게되고 이 값이 -1이 되는 경우가 있다.
            이떄 총알이 샆 처럼 사라지지 않고 계속 씬에 남아있는 경우가 있다.
            샆의 관통력을 -100으로 바꾼다.
            EnenyCleaner의 per 값도 -100으로 바꾼다.
        [d]. 총알이 생성되어 날아갈 때의 제어문으로 0보다 크거나 같게 한다.
            if(per >= 0)
        [e]. 총알이 충돌할 때 관통력을 상실해 가다가 소멸되는데 샆의 관통력 -1일 때는 그냥 반환하는 제어문이 있다.
            -100으로 바꿔준다.

    #4. 투사체 삭제 추가
        [a]. 총알이 몬스터와 충돌하지 않았을 때 맵 밖까지 끝없이 날라간다.
        [b]. Bullet 스크립트에서 충돌 탈출 이벤트 함수를 만든다.
            플레이어의 Area를 탈출 했을 때 사라지게 한다.

    #5. 레벨 디자인
        [a]. Spawner 스크립트 : 몬스터 소환 로직
        [b]. Update() 함수에서 스폰 레벨에 따른 몬스터 소환
            시간에 따라 레벨이 오르는데 레벨의 기준이 될 정수형 속성을 만든다.
            public float levelTime;
        [c]. Awake() 함수에서 초기화 한다.
            최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
                levelTime = GameManager.instance.maxGameTime / spawnData.Length;
*/

/*
21. 뱀서라이크 - 모바일 빌드하기

    #1. 조이스틱 추가
        [a]. 화먼에 조이스틱을 만든다.
        [b]. 인풋 매니저의 로직을 통해 플레이어를 이동 시킨다.
        [c]. 패키지 매니저에서 인풋 매니저 안에 On-Screen Control을 임포트 한다.
        [d]. 캔버스에 조이스틱의 테두리 이미지가 될 이미지 오브젝트를 만든다. Joy
            조이스틱 스프라이트를 넣는다.
            온스크린 컨트롤의 Stick을 Joy의 자식으로 넣어 준다.
            Stick에 조이스틱 스프라이트를 넣는다.
                위치 값을 0/0
                하이어라키 창에서 스틱 우클릭 -> 언팩 컨플리틀리
                스틱의 자식 text는 지운다.
            Joy의 앵커를 아래로
        [e]. Stick의 컴포넌트 On-Screen Stick에 보면 Movement Range가 있는데 조이스틱을 끌어 당길 때 이미지가 움직이는 크기를 조절한다.
            10으로 지정
            Control Path이 Player의 Move와 일치한지 확인
        [f]. Player의 컴포넌트 Player Input을 보면 Auto-Switch 가 되어 있는데 이는 디바이스에 따라 자동으로 입력을 지정해 준다.
            체크 해제
            Default Scheme을 GamePad로 지정
        [g]. Joy의 Scale 값을 0으로
        [h]. 게임 매니저에서 조이스틱을 활성화 하도록 한다.
            조이스틱 오브젝트 변수 추가한다.    public Transform uiJoy;
        [i]. Stop() 함수에서 숨기고 Resume() 함수에서 띄운다.
            uiJoy.localScale = Vector3.zero; one;

    #2. 종료 버튼 만들기
        [a]. GameStart의 자식으로 버튼을 만든다.
            판넬 스프라이트 지정
            GameStart의 앵커를 가득 채움으로 바꾸고 버튼의 앵커를 아래로 내린다.
                size 102/15, 폰트 변경, 
                    size 7, 사냥 종료, 색 변경
            버튼에 Shadow 컴포넌트 부착, Quit로 명명
        [b]. 게임 매니저에서 종료 버튼에 연결할 함수를 만든다.
            Application.Quit();

    #3. 렌더러와 프레임 지정
        [a]. Project Setting -> Quality
            미디움을 제외한 나머지 삭제
            랜더 파이프라인 에셋 : 유니버셜 렌더 파이프라인 에셋으로 연결
            VSync Count : Don't Sync
        [b]. 게임 매니저에서 프레임을 지정한다.
            Awake() 함수에서 Application으로 프레임을 직접 설정한다.
                Application.targetFrameRate = 60;

    #4. 포스트 프로세싱
        [a]. 하이어라키 -> Volume -> Gloval Volume
            컴포넌트에 프로필을 새롭게 추가한다.
            Add Override -> Bloom, Flim Grain, Vignette
        [b]. MainCamera의 컴포넌트 Rendering의 Post Processing을 체크하여 후처리를 적용한다.
        [c]. 다시 Gloval Volume의 컴포넌트를 설정한다.
            Bloom : 빛 번짐 효과
                Threshold=0.95, intensity=5, Scatter=0.35 체크
            Flim Grain : 필름 노이즈 효과
                Type, intensity=0.3, response=0.7 체크
            Vignette : 모서리 음영 처리 효과
                Color, Center, intensity=0.35, Smoothness=0.2 체크
        [d]. Volume의 Weight를 통해 후처리 효과 전체를 조정할 수 있다.

    #5. 모바일 시뮬레이터
        [a]. Game -> Game -> Simulator
        [b]. 기기를 보고 HUD 위치와 크기를 재조정

    #6. 모바일 빌드
        [a]. 빌드 세팅으로 간다.
        [b]. 안드로이드로 스위치 플랫폼
        [c]. 플레이어 세팅 에서 회사 이름과 게임 이름 지정
            아이콘 Icon
        [d]. 리솔루션 프리센테이션 -> 하이드 네비게이션 바 해제
            랜드스케이프 체크 해제
        [e]. 스플레시 이미지 -> 스플레시 스타일 지정
        [f]. 아더 세팅 -> 스크립트 백렌드를 IL2CPP로 지정하여 64비트로
            ARM64 체크
*/