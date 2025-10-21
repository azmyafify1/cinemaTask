using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinemaTask.Migrations
{
    /// <inheritdoc />
    public partial class addDataToCinemasModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        INSERT INTO Cinemas (Name, ImgPath) VALUES
        ('Ludwig Chalfant', 'UltricesPosuere.xls'),
        ('Ami Brecken', 'MorbiSem.xls'),
        ('Rex Duckworth', 'EtErosVestibulum.avi'),
        ('Birgitta Mathen', 'AccumsanTortor.txt'),
        ('Kirby Birdwhistle', 'Quam.ppt'),
        ('Georgia Liverseege', 'Integer.pdf'),
        ('Mattie Menelaws', 'NequeVestibulumEget.xls'),
        ('Page Tows', 'BlanditMi.xls'),
        ('Emilie Fencott', 'Luctus.jpeg'),
        ('Sarita Imlacke', 'LuctusNecMolestie.mpeg'),
        ('Lynett Powling', 'AcDiam.avi'),
        ('Say Pretsell', 'TinciduntIn.mov'),
        ('Tadd Munnion', 'Massa.avi'),
        ('Willy Connerly', 'LobortisLigula.ppt'),
        ('Allegra Wasson', 'Bibendum.ppt'),
        ('Rochester Tivers', 'Pellentesque.ppt'),
        ('Crista Leale', 'EtMagnis.mpeg'),
        ('Jacqueline Duguid', 'UllamcorperPurusSit.xls'),
        ('Rollie Vaughton', 'PellentesqueAtNulla.avi'),
        ('Anna Chanders', 'Molestie.doc');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Cinemas");

        }
    }
}
