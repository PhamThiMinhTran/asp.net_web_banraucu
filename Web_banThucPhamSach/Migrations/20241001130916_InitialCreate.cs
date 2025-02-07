using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_banThucPhamSach.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83F32124B4C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    status = table.Column<int>(type: "int", nullable: true),
                    total_money = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3213E83F8A0614B7", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83FFF252E8C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phoneNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__3213E83FE196FB35", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    discount = table.Column<double>(type: "float", nullable: true),
                    image = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description_dish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83FE12EAD7B", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product__categor__1B0907CE",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    order_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    change_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderHis__3213E83F7EAC01A3", x => x.id);
                    table.ForeignKey(
                        name: "FK__OrderHist__order__3D5E1FD2",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    payment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    transaction_id = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    total_amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentO__3213E83FEE429334", x => x.id);
                    table.ForeignKey(
                        name: "FK__PaymentOp__order__403A8C7D",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    order_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    shipping_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tracking_number = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    shipping_fee = table.Column<double>(type: "float", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipping__3213E83F3E78312C", x => x.id);
                    table.ForeignKey(
                        name: "FK__Shipping__order___398D8EEE",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__staff__3213E83F8F2ABAD2", x => x.id);
                    table.ForeignKey(
                        name: "FK__staff__role_id__17F790F9",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    user_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83FA966300D", x => x.id);
                    table.ForeignKey(
                        name: "FK__User__role_id__145C0A3F",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    image = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gallery__3213E83FB9026C00", x => x.id);
                    table.ForeignKey(
                        name: "FK__Gallery__product__1DE57479",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "InputWarehouse",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    suppliers_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    nameProduct = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    numberInput = table.Column<int>(type: "int", nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    dateIn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__InputWar__3213E83F8D515E03", x => x.id);
                    table.ForeignKey(
                        name: "FK__InputWare__produ__71D1E811",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__InputWare__suppl__72C60C4A",
                        column: x => x.suppliers_id,
                        principalTable: "Suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    discount_percentage = table.Column<double>(type: "float", nullable: true),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Promotio__3213E83FA6CE7E43", x => x.id);
                    table.ForeignKey(
                        name: "FK__Promotion__produ__2A4B4B5E",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    user_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    number = table.Column<int>(type: "int", nullable: true),
                    allTotal = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__3213E83F5DBB4CE7", x => x.id);
                    table.ForeignKey(
                        name: "FK__Cart__product_id__787EE5A0",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart__user_id__797309D9",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    user_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3213E83F34BC4EC7", x => x.id);
                    table.ForeignKey(
                        name: "FK__Feedback__user_i__22AA2996",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "NewsletterSubscription",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    subscribe_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Newslett__3213E83FAC7526A9", x => x.id);
                    table.ForeignKey(
                        name: "FK__Newslette__user___440B1D61",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    user_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__3213E83F4A1D66F0", x => x.id);
                    table.ForeignKey(
                        name: "FK__Review__product___33D4B598",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Review__user_id__34C8D9D1",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    inputWarehose_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    number_product = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Warehous__3213E83F2451DF4C", x => x.id);
                    table.ForeignKey(
                        name: "FK__Warehouse__input__75A278F5",
                        column: x => x.inputWarehose_id,
                        principalTable: "InputWarehouse",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    order_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    product_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    discount = table.Column<double>(type: "float", nullable: true),
                    number_products = table.Column<int>(type: "int", nullable: true),
                    total_money = table.Column<double>(type: "float", nullable: true),
                    promotion_id = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3213E83FC494DA14", x => x.id);
                    table.ForeignKey(
                        name: "FK__OrderDeta__order__2D27B809",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__produ__2F10007B",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__promo__2E1BDC42",
                        column: x => x.promotion_id,
                        principalTable: "Promotion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_product_id",
                table: "Cart",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_user_id",
                table: "Cart",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_user_id",
                table: "Feedback",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_product_id",
                table: "Gallery",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_InputWarehouse_product_id",
                table: "InputWarehouse",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_InputWarehouse_suppliers_id",
                table: "InputWarehouse",
                column: "suppliers_id");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscription_user_id",
                table: "NewsletterSubscription",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_order_id",
                table: "OrderDetails",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_product_id",
                table: "OrderDetails",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_promotion_id",
                table: "OrderDetails",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_order_id",
                table: "OrderHistory",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_order_id",
                table: "PaymentOptions",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_product_id",
                table: "Promotion",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Review_product_id",
                table: "Review",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Review_user_id",
                table: "Review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_order_id",
                table: "Shipping",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_role_id",
                table: "staff",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_inputWarehose_id",
                table: "Warehouse",
                column: "inputWarehose_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "NewsletterSubscription");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "PaymentOptions");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "InputWarehouse");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
