using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFitnessProfilePrimaryKeyToUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$
                DECLARE mismatched bigint;
                BEGIN
                UPDATE "Workouts" AS t SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id" AND t."OwnerId" IS NULL;
                UPDATE "WeightEntries" AS t SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id" AND t."OwnerId" IS NULL;
                UPDATE "Exercises" AS t SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id" AND t."OwnerId" IS NULL;
                UPDATE "WorkoutTemplates" AS t SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id" AND t."OwnerId" IS NULL;

                SELECT count(*) INTO mismatched FROM (
                    SELECT 1 FROM "Workouts" AS t
                    LEFT JOIN "FitnessProfiles" AS fp ON fp."Id" = t."FitnessProfileId"
                    WHERE t."OwnerId" IS DISTINCT FROM fp."UserId"
                    UNION ALL
                    SELECT 1 FROM "WeightEntries" AS t
                    LEFT JOIN "FitnessProfiles" AS fp ON fp."Id" = t."FitnessProfileId"
                    WHERE t."OwnerId" IS DISTINCT FROM fp."UserId"
                    UNION ALL
                    SELECT 1 FROM "Exercises" AS t
                    LEFT JOIN "FitnessProfiles" AS fp ON fp."Id" = t."FitnessProfileId"
                    WHERE t."OwnerId" IS DISTINCT FROM fp."UserId"
                    UNION ALL
                    SELECT 1 FROM "WorkoutTemplates" AS t
                    LEFT JOIN "FitnessProfiles" AS fp ON fp."Id" = t."FitnessProfileId"
                    WHERE t."OwnerId" IS DISTINCT FROM fp."UserId"
                ) AS x;

                IF mismatched > 0 THEN
                    RAISE EXCEPTION 'Aborting migration: % row(s) have an OwnerId that does not match their FitnessProfile.UserId', mismatched;
                END IF;

                IF EXISTS (
                    SELECT 1 FROM "Exercises"
                    WHERE "OwnerId" IS NULL AND "IsDeleted" = FALSE
                    GROUP BY "Name", "ExerciseCategoryId"
                    HAVING count(*) > 1
                ) THEN
                    RAISE EXCEPTION 'Aborting migration: duplicate system exercises would violate IX_Exercises_Name_ExerciseCategoryId';
                END IF;
                END $$;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_FitnessProfileId",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplates_FitnessProfileId_Name",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles");

            migrationBuilder.DropIndex(
                name: "IX_FitnessProfiles_UserId",
                table: "FitnessProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_FitnessProfileId",
                table: "Exercises");

            // Postgres implicitly dropped this index when Exercises.UserId was removed (20260514184841),
            // so it may not exist even though the model snapshot still tracked it.
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "IX_Exercises_Name_ExerciseCategoryId";""");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "WorkoutTemplates");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FitnessProfiles");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Workouts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_OwnerId_Name",
                table: "WorkoutTemplates",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_OwnerId",
                table: "Workouts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_OwnerId",
                table: "WeightEntries",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises",
                columns: new[] { "Name", "ExerciseCategoryId" },
                unique: true,
                filter: "\"OwnerId\" IS NULL AND \"IsDeleted\" = FALSE");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_OwnerId",
                table: "Exercises",
                columns: new[] { "Name", "OwnerId" },
                unique: true,
                filter: "\"IsDeleted\" = FALSE");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_OwnerId",
                table: "Exercises",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_FitnessProfiles_OwnerId",
                table: "Exercises",
                column: "OwnerId",
                principalTable: "FitnessProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_OwnerId",
                table: "WeightEntries",
                column: "OwnerId",
                principalTable: "FitnessProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_OwnerId",
                table: "Workouts",
                column: "OwnerId",
                principalTable: "FitnessProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_OwnerId",
                table: "WorkoutTemplates",
                column: "OwnerId",
                principalTable: "FitnessProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_FitnessProfiles_OwnerId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_OwnerId",
                table: "WeightEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_OwnerId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_OwnerId",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplates_OwnerId_Name",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_OwnerId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_OwnerId",
                table: "WeightEntries");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_OwnerId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_OwnerId",
                table: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles");

            // Adding an identity column numbers the existing rows automatically.
            migrationBuilder.Sql("""ALTER TABLE "FitnessProfiles" ADD "Id" integer GENERATED BY DEFAULT AS IDENTITY NOT NULL;""");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FitnessProfiles_UserId",
                table: "FitnessProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "Workouts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "WeightEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "Exercises",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "WorkoutTemplates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Workouts" AS t SET "FitnessProfileId" = fp."Id"
                FROM "FitnessProfiles" AS fp
                WHERE t."OwnerId" = fp."UserId";
                UPDATE "WeightEntries" AS t SET "FitnessProfileId" = fp."Id"
                FROM "FitnessProfiles" AS fp
                WHERE t."OwnerId" = fp."UserId";
                UPDATE "Exercises" AS t SET "FitnessProfileId" = fp."Id"
                FROM "FitnessProfiles" AS fp
                WHERE t."OwnerId" = fp."UserId";
                UPDATE "WorkoutTemplates" AS t SET "FitnessProfileId" = fp."Id"
                FROM "FitnessProfiles" AS fp
                WHERE t."OwnerId" = fp."UserId";
                UPDATE "Users" AS u SET "FitnessProfileId" = fp."Id"
                FROM "FitnessProfiles" AS fp
                WHERE u."Id" = fp."UserId";
                UPDATE "Users" SET "FitnessProfileId" = 0 WHERE "FitnessProfileId" IS NULL;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "Workouts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Workouts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_FitnessProfileId_Name",
                table: "WorkoutTemplates",
                columns: new[] { "FitnessProfileId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_FitnessProfileId",
                table: "Exercises",
                columns: new[] { "Name", "FitnessProfileId" },
                unique: true,
                filter: "\"IsDeleted\" = FALSE");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_FitnessProfileId",
                table: "WorkoutTemplates",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
