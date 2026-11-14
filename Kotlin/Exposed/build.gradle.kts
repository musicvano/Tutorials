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
    val exposed = "1.4.0"
    implementation("org.jetbrains.exposed:exposed-core:$exposed")
    implementation("org.jetbrains.exposed:exposed-jdbc:$exposed")
    implementation("org.jetbrains.exposed:exposed-dao:$exposed")
    implementation(
        "org.jetbrains.exposed:exposed-java-time:$exposed"
    )
    implementation("org.xerial:sqlite-jdbc:3.53.1.0")
    implementation(
        "org.jetbrains.kotlinx:kotlinx-coroutines-core:1.11.0"
    )
}

// Example 2 of the lecture; ShelvesKt (example 1) and sqllog.ShelvesLogKt
// (screenshot 14/05) run from the gutter icon next to their main.
application { mainClass.set("CatalogKt") }

// Screenshot 14/06: writes Exposed\library.db for DataGrip.
tasks.register<JavaExec>("libraryDb") {
    group = "application"
    description = "Creates library.db (authors, books) for DataGrip."
    classpath = sourceSets["main"].runtimeClasspath
    mainClass.set("librarydb.LibraryDbKt")
    javaLauncher.set(javaToolchains.launcherFor {
        languageVersion.set(JavaLanguageVersion.of(27))
    })
    workingDir = projectDir
    systemProperty("library.db", file("library.db").absolutePath)
}
