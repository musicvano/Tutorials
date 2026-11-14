// Plugin versions of the lectures (01, 14: Kotlin 2.4.20; 15: Compose 1.11.1;
// 16: serialization 2.4.20), declared once here so every topic subproject shares
// one plugin classloader. Serialization is an included build with its own versions.
plugins {
    kotlin("jvm") version "2.4.20" apply false
    kotlin("plugin.serialization") version "2.4.20" apply false
    id("org.jetbrains.kotlin.plugin.compose") version "2.4.20" apply false
    id("org.jetbrains.compose") version "1.11.1" apply false
}

// `gradlew build` / `gradlew test` also build and test the included Serialization build.
tasks.register("build") {
    dependsOn(gradle.includedBuild("Serialization").task(":build"))
}
tasks.register("test") {
    dependsOn(gradle.includedBuild("Serialization").task(":test"))
}
