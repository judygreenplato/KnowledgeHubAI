using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentEmbeddingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DocumentEmbeddings_DocumentChunkId",
                table: "DocumentEmbeddings",
                column: "DocumentChunkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentEmbeddings_DocumentChunks_DocumentChunkId",
                table: "DocumentEmbeddings",
                column: "DocumentChunkId",
                principalTable: "DocumentChunks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentEmbeddings_DocumentChunks_DocumentChunkId",
                table: "DocumentEmbeddings");

            migrationBuilder.DropIndex(
                name: "IX_DocumentEmbeddings_DocumentChunkId",
                table: "DocumentEmbeddings");
        }
    }
}
