import org.jetbrains.compose.desktop.application.dsl.TargetFormat
import org.jetbrains.kotlin.gradle.dsl.JvmTarget

plugins {
    kotlin("jvm")
    id("org.jetbrains.kotlin.plugin.compose")
    id("org.jetbrains.compose")
    kotlin("plugin.serialization")
}
repositories {
    google()
    mavenCentral()
}
kotlin {
    jvmToolchain(27)
    compilerOptions { jvmTarget.set(JvmTarget.JVM_26) }
}
tasks.withType<JavaCompile>().configureEach {
    options.release.set(26)
}
dependencies {
    implementation(compose.desktop.currentOs)
    implementation("org.jetbrains.compose.material3:material3:1.9.0")
    implementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-swing:1.11.0"
    )
    implementation(
        "org.jetbrains.androidx.lifecycle:" +
            "lifecycle-viewmodel-compose:2.10.0"
    )
    implementation(
        "org.jetbrains.androidx.navigation:navigation-compose:2.9.2"
    )
    implementation(
        "org.jetbrains.kotlinx:kotlinx-serialization-json:1.9.0"
    )
    implementation("org.jetbrains.exposed:exposed-core:1.4.0")
    implementation("org.jetbrains.exposed:exposed-jdbc:1.4.0")
    implementation("org.xerial:sqlite-jdbc:3.53.1.0")
    testImplementation(kotlin("test"))
    testImplementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-test:1.11.0"
    )
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
}
tasks.test { useJUnitPlatform() }

// Example 3 (persistent notes) is Main.kt; examples 1 and 2 (Counter.kt,
// Navigation.kt) run from the gutter icon next to their main.
compose.desktop {
    application {
        mainClass = "MainKt"
        // Not a lecture listing: the lecture names nativeDistributions without
        // a listing. Screenshot 16/09 packages with the JDK 27 toolchain
        // (Gradle itself runs on JDK 25, the bytecode targets 26).
        javaHome = javaToolchains.launcherFor {
            languageVersion.set(JavaLanguageVersion.of(27))
        }.get().metadata.installationPath.asFile.absolutePath
        nativeDistributions {
            targetFormats(TargetFormat.Msi)
            packageName = "Notes"
            packageVersion = "1.0.0"
        }
    }
}
