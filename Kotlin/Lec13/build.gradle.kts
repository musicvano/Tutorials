import org.jetbrains.kotlin.gradle.dsl.JvmTarget

plugins {
    kotlin("jvm")
    application
}

repositories { mavenCentral() }

kotlin {
    jvmToolchain(27)
    compilerOptions { jvmTarget.set(JvmTarget.JVM_26) }
}

tasks.withType<JavaCompile>().configureEach {
    options.release.set(26)
}

dependencies {
    implementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-core:1.11.0"
    )
    testImplementation(kotlin("test"))
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
    testImplementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-test:1.11.0"
    )
}
tasks.test { useJUnitPlatform() }

// Example 1 of the lecture; the other programs run from the gutter icon next
// to their main (the lecture swaps them into Main.kt one at a time).
application { mainClass.set("BreakfastKt") }
tasks.named<JavaExec>("run") {
    standardInput = System.`in`
}
