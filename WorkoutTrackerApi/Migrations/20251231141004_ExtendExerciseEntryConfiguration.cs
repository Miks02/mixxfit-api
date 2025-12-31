using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class ExtendExerciseEntryConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_CaloriesBurned_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_DistanceKm_Positive",
                table: "ExerciseEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkoutDate",
                table: "Workouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "PaceMinPerKm",
                table: "ExerciseEntries",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "DistanceKm",
                table: "ExerciseEntries",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CaloriesBurned",
                table: "ExerciseEntries",
                type: "float(7)",
                precision: 7,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_IntervalsCount_Positive",
                table: "ExerciseEntries",
                sql: "IntervalsCount > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_GreaterThan_AvgHeartRate",
                table: "ExerciseEntries",
                sql: "MaxHeartRate >= AvgHeartRate");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_Positive",
                table: "ExerciseEntries",
                sql: "MaxHeartRate > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_RestIntervalSec_Positive",
                table: "ExerciseEntries",
                sql: "RestIntervalSec > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_WorkIntervalSec_Positive",
                table: "ExerciseEntries",
                sql: "WorkIntervalSec > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_IntervalsCount_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_GreaterThan_AvgHeartRate",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_RestIntervalSec_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_WorkIntervalSec_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "WorkoutDate",
                table: "Workouts");

            migrationBuilder.AlterColumn<double>(
                name: "PaceMinPerKm",
                table: "ExerciseEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "DistanceKm",
                table: "ExerciseEntries",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CaloriesBurned",
                table: "ExerciseEntries",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(7)",
                oldPrecision: 7,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_CaloriesBurned_Positive",
                table: "ExerciseEntries",
                sql: "CaloriesBurned > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_DistanceKm_Positive",
                table: "ExerciseEntries",
                sql: "DistanceKm > 0");
        }
    }
}
