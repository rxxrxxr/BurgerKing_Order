using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;

namespace OrderForm
{
    public partial class Main_form : Form
    {
        string server = "192.168.0.38"; // HeidiSQL 서버 주소
        string database = "BurgerKing"; // 데이터베이스 이름
        string uid = "root"; // 사용자 이름
        string password = "1234"; // 암호

        string connectionString;
        MySqlConnection connection;


        public Main_form()
        {
            InitializeComponent();
        }
        //----------------------- 각 메뉴(메인,사이드,드링크)페이지 열기 start ------------------------
        private void Add_Form_To_Panel(Form form) // Form타입의 매개변수 form을 입력받는 메소드
        {
            // 전달받은 폼을 패널에 추가하고 화면에 표시.
            form.TopLevel = false; // 폼을 부를때 TopLevel이면 다른패널에서 못씀, 그래서 false로 정의
            form.FormBorderStyle = FormBorderStyle.None; // form의 테두리 없애기
            form.Dock = DockStyle.Fill; // 패널에 꽉차게 하기
            panel1.Controls.Clear(); // 이미 추가된 폼이 있으면 제거.
            panel1.Controls.Add(form); // 패널에 추가
            form.Show(); // 화면에 표시
        }
        private void Btn_Main_Menu_Click(object sender, EventArgs e) // 메인 메뉴 버튼을 누르면 버거 폼 불러오기
        {
            // OrderForm.Main 네임스페이스의 Burger 클래스에 접근하여 인스턴스를 생성.
            OrderForm.product_menu.Burger burger = new OrderForm.product_menu.Burger(this);

            // burger을 패널에 추가하고 화면에 표시.
            Add_Form_To_Panel(burger);
        }
        private void Btn_Side_Menu_Click(object sender, EventArgs e) // 메인 메뉴 버튼을 누르면 사이드 폼 불러오기
        {
            OrderForm.product_menu.Side side = new OrderForm.product_menu.Side(this);
            Add_Form_To_Panel(side);
        }
        private void Btn_Drink_Menu_Click(object sender, EventArgs e) // 메인 메뉴 버튼을 누르면 드링크 폼 불러오기
        {
            OrderForm.product_menu.Drink drink = new OrderForm.product_menu.Drink(this);
            Add_Form_To_Panel(drink);
        }
        //----------------------- 각 메뉴(메인,사이드,드링크)페이지 열기 end ------------------------



        // ---------------------- 메뉴 선택하면 listview에 보여주기 start ---------------------------
        public void Load_Products_calum(int menu_id)
        {
            connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};"; // DB 접속
            connection = new MySqlConnection(connectionString);
            connection.Open(); // DB 연결 시작
            try
            {
                string query = $"SELECT menu_name, stock, price FROM menu WHERE menu_id = {menu_id}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                // ListView에 데이터 추가
                while (reader.Read())
                {
                    string productName = reader.GetString("menu_name");
                    int productPrice = reader.GetInt32("price");

                    ListViewItem existingItem = Burger_Order_Listview.FindItemWithText(productName);

                    if (existingItem != null)
                    {
                        // 이미 존재하는 제품인 경우 price와 quantity를 증가시킴
                        int currentQuantity = int.Parse(existingItem.SubItems[1].Text); // 현재 quantity 가져오기
                        int newQuantity = currentQuantity + 1; // quantity 증가
                        existingItem.SubItems[1].Text = newQuantity.ToString(); // 증가된 quantity 설정

                        int currentPrice = int.Parse(existingItem.SubItems[2].Text); // 현재 price 가져오기

                    }
                    else
                    {
                        // 새로운 제품인 경우 ListView에 추가
                        ListViewItem item = new ListViewItem(productName);
                        item.SubItems.Add("1"); // quantity를 항상 1로 설정
                        item.SubItems.Add(productPrice.ToString());
                        Burger_Order_Listview.Items.Add(item);
                    }
                    Update_Total_Price();


                }
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 연결 오류: " + ex.Message);
            }
            finally
            {
                // DB 연결 닫기
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }
        // ---------------------- 메뉴 선택하면 listview에 보여주기 start ---------------------------



        // ---------------------- listview 목록 삭제하기 start -----------------------------
        private void Delete_Selected_Menu()
        {
            if (Burger_Order_Listview.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = Burger_Order_Listview.SelectedItems[0]; // SelectedItems[0]는 메뉴이름, 메뉴이름을 선택하고 목록을 삭제한다
                int currentQuantity = int.Parse(selectedItem.SubItems[1].Text);
                if (currentQuantity > 1)
                {
                    // quantity가 1 이상인 경우 quantity를 감소시킴
                    currentQuantity--;
                    selectedItem.SubItems[1].Text = currentQuantity.ToString();
                }
                else
                {
                    // quantity가 1인 경우 ListView에서 행을 제거함
                    Burger_Order_Listview.Items.Remove(selectedItem);
                }

                Update_Total_Price();
            }
            else
            {
                MessageBox.Show("취소할 제품을 선택하세요.");
            }
        }
        private void Btn_Cancel_all_Click(object sender, EventArgs e) // 주문 취소를 누르면 listView를 초기화 해준다 (전체 삭제 버튼)
        {
            Burger_Order_Listview.Items.Clear();
            Update_Total_Price();
        }
        private void Btn_Cancel_Selection_Click(object sender, EventArgs e) // 선택 삭제 버튼
        {
            Delete_Selected_Menu();
        }
        // ---------------------- listview 목록 삭제하기 end -----------------------------



        // ---------------------- 총 금액 계산 start--------------------------------
        private void Update_Total_Price()
        {
            int totalPrice = 0;

            foreach (ListViewItem item in Burger_Order_Listview.Items)
            {
                int quantity = int.Parse(item.SubItems[1].Text); // 제품의 수량
                int price = int.Parse(item.SubItems[2].Text); // 제품의 가격
                totalPrice += quantity * price;// 가격 * 수량을 총 가격에 더함
            }

            Lb_Total_Price.Text = totalPrice.ToString(); // 총 금액을 Label에 표시
        }
        // ---------------------- 총 금액 계산 end ----------------------------------



        // ---------------------- 오더테이블에 정보 넣기 start ------------------------
        
        private int Save_Order() // order 테이블에 order_id와 order_time을 저장하는 메서드
        {
            int orderId = 0; // 초기 주문 ID 설정 0으로 초기화
            try
            {
                string query = $"INSERT INTO `order` (order_time) VALUES (UTC_TIMESTAMP())"; // order 테이블에있는 order_time 컬럼에 NOW를 넣으면 현재시간이 등록된다
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery(); // 쿼리 실행문

                // 새로 생성된 order_id 가져오기
                query = "SELECT LAST_INSERT_ID()"; // MySQL에서 마지막으로 삽입된 자동 증가(primary key) 열의 값을 가져오는 쿼리
                command = new MySqlCommand(query, connection);
                // command.ExecuteScalar() 메서드를 통해 쿼리를 실행하고
                // 그 결과로 반환된 첫 번째 행의 첫 번째 열의 값을 가져온다
                // 그다음 Convert.ToInt32()를 사용하여 정수로 변환한 다음 orderId 변수에 넣는다.
                // 새로운 주문이 들어왔을때 생성된 order_id 값을 가져오는데 사용
                orderId = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException ex) // 예외처리
            {
                MessageBox.Show("주문을 처리하는 동안 오류가 발생했습니다: " + ex.Message);
            }
            return orderId;
        }
        private int Get_MenuId_By_Name(string menuName) // 메뉴 이름을 기반으로 메뉴 ID를 가져오는 메서드
        {
            int menuId = 0; // 초기 메뉴 ID 설정
            try
            {
                // menu 테이블에서 listView에서 받아온 menuName와 똑같은 메뉴이름을 찾아
                // menu_id를 특정한다
                string query = $"SELECT menu_id FROM menu WHERE menu_name = '{menuName}'"; 
                MySqlCommand command = new MySqlCommand(query, connection);
                object result = command.ExecuteScalar();
                // result가 null이 아닐경우(메뉴 이름을 찾은 경우)
                // menuId에 int로 변환한 result를 넣는다
                if (result != null)
                {
                    menuId = Convert.ToInt32(result);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("메뉴 ID를 가져오는 동안 오류가 발생했습니다: " + ex.Message);
            }
            return menuId;
        }

        // 주문된 메뉴 정보를 order_menu 테이블에 저장하는 메서드
        private void Save_Order_Menu(int orderId, int menuId, int quantity)
        {
            try
            {
                // order_menu 테이블에 위에서 정의한 orderId, menuId, 새로 받을 quantity 정보를 저장한다
                string query = $"INSERT INTO order_menu (order_id, menu_id, quantity) VALUES ({orderId}, {menuId}, {quantity})";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery(); // 쿼리 실행
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("주문 메뉴를 추가하는 동안 오류가 발생했습니다: " + ex.Message);
            }
        }
        private void Btn_Payment_Click(object sender, EventArgs e) // 결제 버튼 클릭 이벤트
        {
            connection.Open();
            try
            {
                int orderId = Save_Order(); // Save_Order 메서드를 호출한다

                if (orderId > 0) // 0보다 클때
                {
                    // 각 메뉴별로 주문된 수량과 메뉴 ID를 가져와서 order_menu 테이블에 저장
                    foreach (ListViewItem item in Burger_Order_Listview.Items)
                    {
                        string menuName = item.SubItems[0].Text;
                        int quantity = int.Parse(item.SubItems[1].Text);

                        // 메뉴 이름으로부터 메뉴 ID를 가져오는 메서드 호출
                        int menuId = Get_MenuId_By_Name(menuName);

                        // order_menu 테이블에 주문 정보 저장
                        Save_Order_Menu(orderId, menuId, quantity);
                    }

                    MessageBox.Show("주문이 성공적으로 완료되었습니다.");
                    // 주문 완료 후 ListView 초기화
                    Burger_Order_Listview.Items.Clear();
                    Update_Total_Price();
                }
                else
                {
                    MessageBox.Show("주문을 처리하는 동안 오류가 발생했습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 연결 오류: " + ex.Message);
            }
            finally
            {
                // DB 연결 닫기
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        // ---------------------- 오더테이블에 정보 넣기 end ------------------------
    }
}
