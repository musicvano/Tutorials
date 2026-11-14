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

application { mainClass.set("PriceKt") }
tasks.named<JavaExec>("run") {
    standardInput = System.`in`
}
