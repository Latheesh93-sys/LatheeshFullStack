using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeLatheeshAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogPostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "UrlHandle",
                table: "BlogPosts",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "BlogPosts",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "FeaturedImageUrl",
                table: "BlogPosts",
                newName: "Source");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "BlogPosts",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "BlogPosts",
                newName: "UrlHandle");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "BlogPosts",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "BlogPosts",
                newName: "FeaturedImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "BlogPosts",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
