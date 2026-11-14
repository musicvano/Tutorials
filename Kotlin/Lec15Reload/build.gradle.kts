// Screenshot 15/09 (staging, not a lecture listing): Compose Hot Reload (bundled
// with the Compose Gradle plugin 1.11.1, task hotRun) needs a JetBrains Runtime of
// the toolchain version. There is no JBR 27, so this copy of example 1 (Main.kt =
// the counter of Lec15) uses toolchain and bytecode 25 and the JBR 25 of
// IntelliJ IDEA; everything else is the Lec15 build file of the lecture.
//   gradlew :Lec15Reload:hotRun "-Porg.gradle.java.installations.paths=C:/Program Files/JetBrains/IntelliJ IDEA 2026.2.3/jbr"
import org.jetbrains.kotlin.gradle.dsl.JvmTarget

plugins {
    kotlin("jvm")
    id("org.jetbrains.kotlin.plugin.compose")
    id("org.jetbrains.compose")
}
repositories {
    google()
    mavenCentral()
}
kotlin {
    jvmToolchain(25)
    compilerOptions { jvmTarget.set(JvmTarget.JVM_25) }
}
tasks.withType<JavaCompile>().configureEach {
    options.release.set(25)
}
dependencies {
    implementation(compose.desktop.currentOs)
    implementation("org.jetbrains.compose.material3:material3:1.9.0")
    implementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-swing:1.11.0"
    )
}
compose.desktop {
    application { mainClass = "MainKt" }
}
