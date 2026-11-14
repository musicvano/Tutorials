pluginManagement {
    repositories {
        gradlePluginPortal()
        google()
        mavenCentral()
    }
}

rootProject.name = "oop-kotlin-code"

include(
    "Intro", "NullSafety", "Functions", "Exceptions", "Classes", "Inheritance",
    "DataClasses", "Delegation", "Generics", "Collections", "Lambdas", "Coroutines",
    "Exposed", "Compose", "ComposeHotReload", "Mvvm"
)
// Serialization (topic 12) is its own build: its build.gradle.kts is the lecture's
// complete file with the plugin versions (screenshot 12/04), which a subproject
// cannot repeat.
includeBuild("Serialization")
