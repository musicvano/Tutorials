pluginManagement {
    repositories {
        gradlePluginPortal()
        google()
        mavenCentral()
    }
}

rootProject.name = "oop-kotlin-code"

include(
    "Lec01", "Lec02", "Lec03", "Lec04", "Lec05", "Lec06", "Lec07", "Lec08",
    "Lec09", "Lec10", "Lec11", "Lec13", "Lec14", "Lec15", "Lec15Reload", "Lec16"
)
// Lec12 is its own build: its build.gradle.kts is the lecture's complete file
// with the plugin versions (screenshot 12/04), which a subproject cannot repeat.
includeBuild("Lec12")
