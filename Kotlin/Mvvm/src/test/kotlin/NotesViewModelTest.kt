import kotlin.test.*
import kotlinx.coroutines.*
import kotlinx.coroutines.test.*

class FakeNotes : NoteRepository {
    val rows = mutableListOf<Note>()
    var fail = false
    override suspend fun all(): List<Note> = rows.toList()
    override suspend fun add(title: String) {
        check(!fail) { "storage unavailable" }
        rows.add(Note(rows.size + 1, title))
    }
    override suspend fun delete(id: Int) {
        rows.removeAll { it.id == id }
    }
}

@OptIn(ExperimentalCoroutinesApi::class)
class NotesViewModelTest {
    @Test
    fun validatesSavesAndKeepsFailedDraft() = runTest {
        Dispatchers.setMain(StandardTestDispatcher(testScheduler))
        try {
            val repository = FakeNotes()
            val model = NotesViewModel(repository)
            model.edit(" ")
            model.add()
            assertTrue(repository.rows.isEmpty())
            assertNotNull(model.state.value.error)
            model.edit("  Перша  ")
            model.add()
            advanceUntilIdle()
            assertEquals("Перша", repository.rows.single().title)
            assertEquals("", model.state.value.draft)
            repository.fail = true
            model.edit("Друга")
            model.add()
            advanceUntilIdle()
            assertEquals("Друга", model.state.value.draft)
            assertNotNull(model.state.value.error)
            assertFalse(model.state.value.busy)
        } finally {
            Dispatchers.resetMain()
        }
    }
}
